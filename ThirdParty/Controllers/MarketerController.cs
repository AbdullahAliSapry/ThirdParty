using AutoMapper;
using Bll.Dtos;
using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Security.Claims;
using System.Threading.Tasks;
using ThirdParty.ViewModels;
using Twilio.TwiML.Messaging;

namespace ThirdParty.Controllers
{
    [Authorize]
    public class MarketerController : Controller
    {


        private IUnitOfWork<Marketer> _unitOfWork { get; set; }

        private IMapper _mapper { get; }
        public MarketerController(IUnitOfWork<Marketer> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [Authorize(Roles = "Markter")]
        public async Task<IActionResult> MainSettingMarketer(string userid)
        {
            // handling all logic


            if (!Guid.TryParse(userid, out Guid IdParse))
            {
                return View("Error", new ErrorViewModel
                {

                    RequestId = "404",
                    Message = "Invalid User Id "
                });
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId is not null && userId.Value.ToString() != userid)
            {
                return View("AccessDenied", new AccessDeniedViewModel
                {

                    Message = "Access Denied To Enter This Page",
                    StatusMessage = "401"
                });
            }


            // 1-get all code to user


            var marketer = await _unitOfWork.Marketer.
                GetItemWithFunc(e => e.UserId == userid, new string[] { "CodesToMarketers.ReferralCodeUsages" });



            var marketerVm = _mapper.Map<MarketerVm>(marketer);



            return View(marketerVm);
        }



        [HttpPost("/Marketer/AddCodeToMarketer/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCodeToMarketer([FromBody] MarketerCodeVm marketerCode, string userId)
        {
            Console.WriteLine("Enter in function");

            if (User.Identity?.IsAuthenticated is true && User.FindFirst(ClaimTypes.NameIdentifier)?.Value != userId)
            {
                return Forbid("You are not authorized to perform this action.");
            }

            if (!ModelState.IsValid)
            {
                var errorMessages = new List<string>();

                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key]?.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            errorMessages.Add($"{key}: {error.ErrorMessage}");
                        }
                    }
                }

                var combinedErrors = string.Join(" | ", errorMessages);
                Console.WriteLine($"Validation Errors: {combinedErrors}");

                return BadRequest(new
                {
                    message = "❌ البيانات غير صالحة.",
                    errors = errorMessages
                });
            }


            var code = Guid.NewGuid().ToString("N")[..8];
            while (_unitOfWork.CodeToMarketer.GetItemsWithFunc(e => e.Code == code).Any())
            {
                code = Guid.NewGuid().ToString("N")[..8];
            }

            var codeMarketer = _mapper.Map<CodesToMarketer>(marketerCode);
            codeMarketer.Code = code;
            codeMarketer.MarketerId = _unitOfWork.Marketer
                .GetItemsWithFunc(e => e.UserId == userId).FirstOrDefault()?.Id ?? 0;

            var result = _unitOfWork.CodeToMarketer.Create(codeMarketer);

            if (!result.Status)
            {
                return StatusCode(500, new { message = "Internal Server Error, please try again later." });
            }

            await _unitOfWork.SaveChangesAsync();

            var codeVm = _mapper.Map<MarketerCodeVm>(codeMarketer);

            return Ok(new
            {
                message = "Code created successfully.",
                code = codeVm.Code,
                //marketerId = codeVm,
                revokedAt = codeVm.RevokedAt,
                discountRate = codeVm.DiscountRate,
                isActive = codeVm.IsActive,
                createAt = codeVm.CreatedAt,
            });
        }

        [Authorize(Roles = "Markter")]
        public async Task<IActionResult> GetAllDetailsToCode(string code, string userId)
        {


            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (UserId != userId)
            {
                return View("AccessDenied", new AccessDeniedViewModel
                {
                    Message = "Access Denied ",
                    StatusMessage = "403",

                });
            }
            //var code=_unitOfWork.Marketer.GetItem()
            //
            var marketer = await _unitOfWork.Marketer.GetItemWithFunc(e => e.UserId == userId);

            var codeData = await _unitOfWork.CodeToMarketer.
                GetItemWithFunc(e => e.Code == code,
                new string[] { "ReferralCodeUsages.ApplicationUser", "ReferralCodeUsages.Order" });




            if (codeData == null)
                return View("Error", new ErrorViewModel
                {
                    Message = "Code Not Found",
                    RequestId = "404"
                });

            codeData.ReferralCodeUsages = codeData.ReferralCodeUsages.Where(e => e.Order.IsPaid).ToList();

            // get code and codeeusage
            // 
            var codedatamapping = _mapper.Map<CodesToMarketerDto>(codeData);



            return View(codedatamapping);
        }


        [Authorize(Roles = "Markter")]
        public async Task<IActionResult> ChangeStatusToCode(string userId, string code)
        {

            Console.WriteLine("Enter in action");
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            if (UserId != userId)
            {
                return View("AcessDenied", new AccessDeniedViewModel
                {

                    Message = "Access Denied",
                    StatusMessage = "401"
                });

            }


            var Code = await _unitOfWork.CodeToMarketer.GetItemWithFunc(e => e.Code == code);

            if (code == null)
            {

                TempData["ErrorMessageCode"] = "Code Not Found";

                return RedirectToAction("MainSettingMarketer", new { userid = UserId });
            }

            Code.IsActive = Code.IsActive ? false : true;

            var result = _unitOfWork.CodeToMarketer.UpdateItem(Code);

            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                TempData["ErrorMessageCode"] = "Failed In Update Code";

                return RedirectToAction("MainSettingMarketer", new { userid = UserId });
            }

            TempData["SuccessMessageCode"] = "Code Status Updated Successfully";

            return RedirectToAction("MainSettingMarketer", new { userid = UserId });
        }


        [Authorize]
        [HttpGet("/Marketer/GetDateToCode/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDateToCode(string code, string userId, double totalPrice)
        {
            Console.WriteLine("Sucess");

            var checkauth = User.FindFirstValue(ClaimTypes.NameIdentifier) == userId;

            if (!checkauth)
                return StatusCode(403, new { Message = "Access Denied" });


            if (string.IsNullOrEmpty(code))
                return BadRequest(new { Message = "Code Is Empety Please Enter Code" });


            var codedata = await _unitOfWork
                .CodeToMarketer.GetItemWithFunc(e => e.Code == code);


            if (codedata == null)
                return NotFound(new { Message = "Code Not Found Please chure from code" });

            if (!codedata.IsActive)
                return BadRequest(new { Message = "Code Not Active" });


            var dicount = 0.0;

            if (codedata.DiscountRate != 0)
            {
                var commision = _unitOfWork.CommissionScheme
                    .GetItemsWithFunc(e => e.IsActive && e.UserType == UserType.Markter && e.UpperLimit > totalPrice && e.LowerLimit < totalPrice).FirstOrDefault();

                if (commision != null && commision.CommissionRate != null && codedata.DiscountRate != null)
                {
                    dicount = (totalPrice * (double)commision.CommissionRate) * ((double)codedata.DiscountRate / 100);

                }

            }


            var codedatamapped = _mapper.Map<MarketerCodeVm>(codedata);
            return Json(new { Message = "Code Apply Success", data = codedatamapped, discount = dicount });

        }


        // delete code from marketer


        [Authorize(Roles = "Markter")]
        public async Task<IActionResult> DeleteCode(string code, int codeId, string userId)
        {



            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
                return Forbid();

            if (codeId == 0 || string.IsNullOrEmpty(code))
                return View("Error", new ErrorViewModel
                {
                    Message = "Invalid Id",
                    RequestId = "404"
                });

            var codedata = await _unitOfWork.CodeToMarketer
                .GetItemWithFunc(e => e.Id == codeId, new string[] { "ReferralCodeUsages" });


            if (codedata.ReferralCodeUsages != null && codedata.ReferralCodeUsages.Count > 0)
            {

                TempData["ErrorMessage"] = "Can't delete this code Because it was used";

                return RedirectToAction("MainSettingMarketer", "Marketer", new { userid = userId });

            }



            var result = await _unitOfWork.CodeToMarketer.DeleteItemWithFunc(e => e.Id == codedata.Id);


            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("MainSettingMarketer", "Marketer", new { userid = userId });

            }
            else
            {
                return RedirectToAction("MainSettingMarketer", "Marketer", new { userid = userId });
            }

        }

        [Authorize(Roles = "Markter")]
        public async Task<IActionResult> PayMentDataToMarketer(string userId)
        {



            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
                return Forbid();



            // total plance to marketer

            var markter = await _unitOfWork.Marketer
                .GetItemWithFunc(e => e.UserId == userId, new string[] { "MarketerAccounts", "CodesToMarketers.ReferralCodeUsages.Order" });


            // Calculate the total discount made from the marketer's commission
            double totalDiscountFromMarketerBalances = markter.CodesToMarketers
                .SelectMany(e => e.ReferralCodeUsages)
                .Where(b => DateTime.UtcNow.Month == b.Order.CreatedAt.Month &&b.Order.IsPaid)
                .Sum(b => (double)b.Order.TotalTaxWithOutMarkerDiscount - b.Order.Tax);
            Console.WriteLine($" Totoal usage to codes to marketer {totalDiscountFromMarketerBalances}");
            // Calculate the total profits for this month that the marketer made for the store.
            double totalBalanceAchievedWithMarketer = markter.CodesToMarketers
                .SelectMany(e => e.ReferralCodeUsages)
                .Where(b => DateTime.UtcNow.Month == b.Order.CreatedAt.Month && b.Order.IsPaid)
                .Sum(b => b.Order.TotalPrice + (double)b.Order.TotalTaxWithOutMarkerDiscount);

            // Commission calculation based on the range of profits achieved
            var commission = _unitOfWork.CommissionScheme
                .GetItemsWithFunc(e => e.UserType == UserType.Markter && e.IsActive)
                .FirstOrDefault(e => e.UpperLimit > totalBalanceAchievedWithMarketer && e.LowerLimit < totalBalanceAchievedWithMarketer);

            double allBalanceToMarketer = commission != null ? (commission.CommissionRate * totalBalanceAchievedWithMarketer)- totalDiscountFromMarketerBalances : 0.0;



            ViewData["totolaBalancetomonthe"] = allBalanceToMarketer;
            // totalbalancd without discount

            // all comissions
            //

            return View(markter);
        }

        [HttpPost("Marketer/AddAccount/{userid}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAccount([FromBody] MarketerAccountDto marketerAccountDto, string userid)
        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userid)
                return Forbid();


            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Validation Error", ModelState });


            var checkAccount = await _unitOfWork.MarketerAccount.GetItemWithFunc(e => e.NumberOfAccount == marketerAccountDto.NumberOfAccount);

            if (checkAccount != null)
                return BadRequest(new { Message = "Account Aready Exist" });


            var newaccount = _mapper.Map<MarketerAccount>(marketerAccountDto);

            var result = _unitOfWork.MarketerAccount.Create(newaccount);

            if (result.Status)
            {
                _unitOfWork.SaveChanges();
                return Json(new { Message = "Account Added Success", account = newaccount });

            }
            else
            {
                return BadRequest(new { Message = "Faild Add Account" });


            }

        }



        [Authorize]
        public async Task<IActionResult> AllBillingToAccount(string userId, string accountid)
        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
                return Forbid();


            if (!Guid.TryParse(accountid, out Guid AccountId))
                return View("Error", new ErrorViewModel
                { Message = "Invalid User Id", RequestId = "401" });





            var account = await _unitOfWork.MarketerAccount
                .GetItemWithFunc(e => e.Id == AccountId, new string[] { "BillingToMarketrs" });



            if (account == null)
                return View();




            return View(account);


        }
    }
}
