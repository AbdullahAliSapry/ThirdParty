using AutoMapper;
using Bll.Dtos;
using Bll.Dtos.ApiDto;
using DAl.IRepository;
using DAl.Models;
using Infrastructure.FileUploadService;
using Infrastructure.FileUploadService.FileUploadOnService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;

namespace ThirdParty.Controllers
{
    [Authorize(Roles = "Admin,SubAdmin")]
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;

        private IUnitOfWork<Order> _unitOfWork { get; set; }
        private IMapper _mapper;
        private IFileUploadUloudinary _fileUploadUloudinary { get; set; }
        private IFileUploadService fileUploadService { get; set; }

        public AdminController(ILogger<AdminController> logger, IUnitOfWork<Order> unitOfWork, IMapper mapper, IFileUploadUloudinary fileUploadUloudinary, IFileUploadService fileUploadService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileUploadUloudinary = fileUploadUloudinary;
            this.fileUploadService = fileUploadService;
        }



        public IActionResult Index()
        {


            // order not reviewd
            var orders = _unitOfWork.Order.GetItemsWithFunc(e => !e.IsPrivewed, new string[] { "ReferralCodeUsage" });

            var users = _unitOfWork.User.GetItems();
            // payments

            var paymentsnotactive = _unitOfWork.PayMentManoul.GetItemsWithFunc(e => !e.IsConfirmed);

            // proplems

            var proplems = _unitOfWork.Proplem.GetItemsWithFunc(e => !e.IsSolved);

            ViewData["Orders"] = orders;
            ViewData["Users"] = users;
            ViewData["Payemnts"] = paymentsnotactive;
            ViewData["Proplems"] = proplems;


            _logger.LogInformation("Data Uploaded Succesfully");

            return View();
        }

        public IActionResult AllUsers()
        {

            var users = _unitOfWork
                .User.GetItems();

            return View(users);
        }


        public async Task<IActionResult> DetailsToUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return View();


            var user = await _unitOfWork.
                User.GetItemWithFunc(e => e.Id == userId, new string[] { "Cart.CartItems.Order", "PayMentManoul" });

            if (user.Cart != null)
            {
                Console.WriteLine("Enter in valid");


                var orders = user.Cart
                              .CartItems
                              .Where(e => e.IsOredered && e.Order != null)
                              .Select(x => x.Order)
                              .Distinct()
                              .ToList();


                ViewData["Orders"] = orders;

                ViewData["Payments"] = user.PayMentManoul;

            }

            return View(user);

        }



        public async Task<IActionResult> AllOrders(bool status = true)
        {


            var ordersReviewd = new List<Order>();


            if (status)
            {
                ordersReviewd = _unitOfWork.Order.GetItemsWithFunc(
                                  e => true,
                                  new string[] { "CartItems" }
                              ).OrderBy(e => e.IsPrivewed == status).ToList();
            }
            else
            {
                ordersReviewd = _unitOfWork.Order.GetItemsWithFunc(
                                                e => e.IsPrivewed == status,
                                                new string[] { "CartItems" }
                                            ).ToList();
            }


            var cartIds = ordersReviewd.SelectMany(o => o.CartItems).Select(ci => ci.CartId).Distinct();

            var cartsWithUsers = _unitOfWork.Cart.GetItemsWithFunc(
                c => cartIds.Contains(c.Id),
                new string[] { "User" }
            ).ToDictionary(c => c.Id);

            foreach (var order in ordersReviewd)
            {
                var firstCartItem = order.CartItems.FirstOrDefault();
                if (firstCartItem != null && cartsWithUsers.ContainsKey(firstCartItem.CartId))
                {
                    firstCartItem.Cart = cartsWithUsers[firstCartItem.CartId];
                }
            }



            return View(ordersReviewd);
        }


        public async Task<IActionResult> DetailsOrder(string OrderId)
        {
            if (!Guid.TryParse(OrderId, out Guid orderId))
                return View();

            var order = await _unitOfWork
                .Order
                .GetItemWithFunc(e => e.Id == orderId, new string[] { "CartItems.Category", "CartItems.AttributeItems" });

            if (order == null || order.CartItems == null || order.CartItems.Count == 0)
                return View();

            var firstCartItem = order.CartItems.FirstOrDefault();

            if (firstCartItem != null)
            {
                var cart = await _unitOfWork.Cart
                    .GetItemWithFunc(e => e.Id == firstCartItem.CartId, new string[] { "User" });

                firstCartItem.Cart = cart;
            }

            return View(_mapper.Map<OrderDto>(order));
        }

        [HttpGet("/Admin/ChangeStatusOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatusOrder([FromQuery] string status, [FromQuery] string OrderId)
        {
            if (!Guid.TryParse(OrderId, out Guid orderid))
                return BadRequest(new { Message = "Invalid Id" });

            var order = await _unitOfWork.Order.GetItemWithFunc(e => e.Id == orderid);
            if (order == null)
            {
                return NotFound(new { Message = "Order Not Found" });
            }

            // تحويل القيمة إلى Boolean بشكل صحيح
            bool newStatus = status.ToLower() == "true";

            order.IsPrivewed = newStatus;
            var result = _unitOfWork.Order.UpdateItem(order);

            if (!result)
                return BadRequest(new { Message = "Failed Change Status" });

            var re = _unitOfWork.SaveChanges();

            if (re)
                return Json(new { Message = "Status Change Successfully", Status = order.IsPrivewed });
            else
                return BadRequest(new { Message = "Failed Change Status" });
        }

        [HttpPost("/Admin/UpdatePriceShipping")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePriceShipping([FromBody] DataUpdatePrice dataUpdatePrice)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Valdation Error", ModelState });

            if (!Guid.TryParse(dataUpdatePrice.OrderId, out Guid orderid))
                return BadRequest(new { Message = "Invalid Id" });

            var order = await _unitOfWork.
                Order.
                GetItemWithFunc(e => e.Id == orderid);

            if (order == null)
            {
                return NotFound(new { Message = "Order Not Found" });
            }

            order.ShippingPrice = dataUpdatePrice.Price;
            var result = _unitOfWork.Order.UpdateItem(order);

            if (!result)
                return BadRequest(new { Message = "Failed Change Price" });

            var re = _unitOfWork.SaveChanges();

            if (re)
                return Json(new { Message = "Price Change Successfully", price = order.ShippingPrice });
            else
                return BadRequest(new { Message = "Failed Change Price" });


        }
        public class DataUpdatePrice
        {
            [Required]
            public string OrderId { get; set; }
            [Required]
            public double Price { get; set; }
        }



        public async Task<IActionResult> AllProplem()
        {

            var proplems = _unitOfWork
                .Proplem.GetItems().OrderBy(e => e.IsSolved).ToList();


            return View(proplems);
        }



        public async Task<IActionResult> MarkAsSolved(int proplemId)
        {

            if (proplemId < 0)
            {
                TempData["ErrorMessage"] = "Invalid Message Id";
                return RedirectToAction("AllProplem", "Admin");
            }


            var proplem = await _unitOfWork.Proplem.GetItemWithFunc(e => e.Id == proplemId);

            if (proplem == null)
            {
                TempData["ErrorMessage"] = "InvalidMessage";
                return RedirectToAction("AllProplem", "Admin");
            }
            proplem.IsSolved = true;

            _unitOfWork.Proplem.UpdateItem(proplem);
            _unitOfWork.SaveChanges();
            TempData["SuccessMessage"] = "InvalidMessage";

            return RedirectToAction("AllProplem", "Admin");


        }


        public async Task<IActionResult> AllPaymnets(bool status = true, string AccountId = null)
        {


            var payments = new List<PayMentManoul>();

            if (status)
            {
                payments = _unitOfWork.PayMentManoul.GetItemsWithFunc(e => true, new string[] { "User", "Account", "FileUploads" });
            }
            else
            {
                payments = _unitOfWork.PayMentManoul.GetItemsWithFunc(e => !e.IsConfirmed, new string[] { "User", "Account", "FileUploads" });

            }

            if (AccountId != null && Guid.TryParse(AccountId, out Guid accountid))
            {
                payments = payments.Where(e => e.AccountId == accountid).ToList();
            }




            return View(payments);
        }



        public async Task<IActionResult> AllMarketers()
        {

            var marketers = _unitOfWork.Marketer
                .GetItemsWithFunc(e => true, new string[] { "CodesToMarketers.ReferralCodeUsages"
                , "ApplicationUser" });

            return View(marketers);
        }

        public async Task<IActionResult> MarketerDetails(string marketerid)
        {
            if (string.IsNullOrEmpty(marketerid))
                return View();


            // get marketers
            var marketer = await _unitOfWork.Marketer
                .GetItemWithFunc(e => e.UserId == marketerid, new string[] { "ApplicationUser.Cart.CartItems" ,
                    "CodesToMarketers.ReferralCodeUsages", "MarketerAccounts.BillingToMarketrs" });



            if (marketer == null)
                return View();

            // get orders
            if (marketer.ApplicationUser.Cart != null)
            {
                Console.WriteLine("Enter in valid");


                var orders = marketer.ApplicationUser.Cart
                              .CartItems
                              .Where(e => e.IsOredered && e.Order != null)
                              .Select(x => x.Order)
                              .Distinct()
                              .ToList();

                ViewData["Orders"] = orders;


                Console.WriteLine($"Odrers Count {orders.Count}");

            }


            return View(marketer);

        }



        public async Task<IActionResult> AllCodeDetails(string code, int codeid)
        {



            var CodeData = await _unitOfWork
                .CodeToMarketer
                .GetItemWithFunc(e => e.Id == codeid,
                new string[] { "Marketer.ApplicationUser", "ReferralCodeUsages.Order", "ReferralCodeUsages.ApplicationUser" });




            return View(CodeData);


        }


        public async Task<IActionResult> AllImagesDynamic()
        {

            var images = _unitOfWork.
                ImagesDynamic.GetItems().OrderBy(e => e.typeImageUpload).ToList();



            return View(images);

        }


        [HttpPost("/Admin/AddImageDynamic")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImageDynamic([FromForm] ImageDynamicData imageDynamicData)
        {
            Console.WriteLine("Enter in iameg");

            if (imageDynamicData.file == null)
            {
                Console.WriteLine("❌ الملف لم يتم إرساله!");
                return BadRequest(new { Message = "لم يتم إرسال أي ملف!" });
            }

            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Valdation Error", ModelState });


            var result = await _fileUploadUloudinary.Upload(imageDynamicData.file);


            if (string.IsNullOrEmpty(result.PublicId))
                return BadRequest(new { Message = result.Message });


            var image = new ImagesDynamic()
            {
                IsActive = true,
                PublicId = result.PublicId,
                Url = result.Url,
                typeImageUpload = imageDynamicData.typeImageUpload == "IsLogo" ? TypeImageUpload.IsLogo : TypeImageUpload.Isadvertisement

            };

            _unitOfWork.ImagesDynamic.Create(image);

            _unitOfWork.SaveChanges();

            return Json(new { Message = "Uploaded Successfully", image });

        }

        public async Task<IActionResult> ChangeStatusImage(int imageId)
        {

            var image = await _unitOfWork.ImagesDynamic.GetItemWithFunc(e => e.Id == imageId);
            image.IsActive = !image.IsActive;
            _unitOfWork.ImagesDynamic.UpdateItem(image);

            var result = _unitOfWork.SaveChanges();


            if (!result)
            {
                TempData["ErrorMessage"] = "Falied Change";
                return RedirectToAction("AllImagesDynamic", "Admin");

            }
            TempData["SuccessMessage"] = "Status Changed Successfully";

            return RedirectToAction("AllImagesDynamic", "Admin");

        }


        public class ImageDynamicData
        {
            public IFormFile file { get; set; }

            public string typeImageUpload { get; set; }

        }


        public async Task<IActionResult> PricesToshipping()
        {

            var result = _unitOfWork.PricesToshipping.GetItems();



            return View(result);


        }


        public async Task<IActionResult> AllCommissionSchemes()
        {

            var allcommsion = _unitOfWork.CommissionScheme.GetItems();

            return View(allcommsion);
        }

        [HttpPost("/Admin/AddNewCommissionSchema")]
        public async Task<IActionResult> AddNewCommissionSchema([FromBody] CommissionSchemeDto commissionSchemeDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Valdation Error", ModelState });

            var commsion = _mapper.Map<CommissionScheme>(commissionSchemeDto);
            var result = _unitOfWork.CommissionScheme.Create(commsion);

            if (result.Status)
            {
                _unitOfWork.SaveChanges();
                return Json(new { Message = "Created Successfully", data = commsion });
            }
            else
            {
                return BadRequest(new { Message = "Created Failed" });

            }



        }


        public async Task<IActionResult> AllBillingToMarketer(string accountId)
        {


            if (!Guid.TryParse(accountId, out Guid AccountId))
                return View();


            var account = await _unitOfWork.MarketerAccount
                .GetItemWithFunc(e => e.Id == AccountId, new string[] { "BillingToMarketrs.FileUploads" });
            if (account == null)
                return View();


            return View(account);

        }
        [HttpPost("/Admin/AddBillingToMarketer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBillingToMarketer([FromForm] BillingToMarketrDto billingToMarketrDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { ModelState = ModelState, Message = "Valdation Error" });



            if (!Guid.TryParse(billingToMarketrDto.MarkterAccountId, out Guid AccountId))
                return BadRequest(new { Message = "Invalid Id " });

            var file = new FileUploads();
            // uplaod file
            if (billingToMarketrDto.Image.Length != 0)
            {
                var result = await fileUploadService.UploadFile(billingToMarketrDto.Image, "BillingMarketer", 10);

                if (result != null)
                {
                    file = new FileUploads()
                    {
                        FileName = result.FileName,
                        FileType = Path.GetExtension(billingToMarketrDto.Image.FileName),
                        FilePath = result.FilePath,
                        UploadedDate = DateTime.UtcNow,

                    };

                    var resultcreate = _unitOfWork.FileUploads.Create(file);
                    if (resultcreate.Status)
                    {
                        _unitOfWork.SaveChanges();
                    }
                    else
                    {
                        return BadRequest(new { Message = "Upload File Filed" });

                    }
                }
                else
                {
                    return BadRequest(new { Message = "Upload File Filed" });

                }

            }
            else
            {
                return BadRequest(new { Message = "Image Is Empety" });

            }

            var billingnew = new BillingToMarketr()
            {
                Amount = billingToMarketrDto.Amount,
                CreateAt = DateTime.UtcNow,
                MarkterAccountId = AccountId,
                FileId = file.Id,
                NameBank = billingToMarketrDto.NameBank,

            };

            _unitOfWork.BillingToMarketr.Create(billingnew);


            _unitOfWork.SaveChanges();



            return Ok(new { Message = "Addedd Successfully" });

        }


    }



}
