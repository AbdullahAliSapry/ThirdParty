using ThirdParty.Utilities;
using AspNetCoreGeneratedDocument;
using AutoMapper;
using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using Infrastructure.SendingEmails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Transactions;
using Twilio.Jwt.AccessToken;
using ThirdParty.ViewModels;
using Infrastructure.FileUploadService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Infrastructure.FileUploadService.FileUploadService;

namespace ThirdParty.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private string _code;
        private List<Country> countries;
        private readonly ISendEmail _emailSender;
        public IUnitOfWork<ApplicationUser> unitOfWork { get; }
        public IAuthRepository AuthRepository { get; }
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IMapper _mapper { get; }
        private IFileUploadService _uploadFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ISendEmail emailSender, IUnitOfWork<ApplicationUser> unitOfWork, IAuthRepository authRepository, IMapper mapper, SignInManager<ApplicationUser> signInManager, IFileUploadService uploadFileService, IWebHostEnvironment webHostEnvironment)
        {
            countries = CountryRepository.Countries;
            _code = string.Empty;
            _emailSender = emailSender;
            this.unitOfWork = unitOfWork;
            AuthRepository = authRepository;
            _mapper = mapper;
            this._signInManager = signInManager;
            _uploadFileService = uploadFileService;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet("/Account/Login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid) return View(loginVm);

            var user = await AuthRepository.LoginMethode(loginVm);
            if (user == null)
            {
                TempData["ErrorMessage"] = "خطأ في الايميل او كلمه المرور";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVm.Password, false, false);

            if (result.Succeeded)
            {
                Console.WriteLine("User is authenticated");
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError(string.Empty, "فشل في تسجيل الدخول.");
            return View(loginVm);
        }


        public IActionResult Register()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        public IActionResult RegisterCompany()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Countries = countries;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCompany(CompanyViewModel companyViewModel)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            Console.WriteLine($"is comapny or shop {companyViewModel.IsComanyOrShop}");

            ViewBag.Countries = countries;

            if (!ModelState.IsValid)
            {
                return View(companyViewModel);
            }

            if (await unitOfWork.UserManager.FindByEmailAsync(companyViewModel.Email) != null)
            {

                ModelState.AddModelError("Email", "البريد الإلكتروني موجود بالفعل");

                return View(companyViewModel);
            }

            if (unitOfWork.UserManager.Users.Any(e => e.PhoneNumber == companyViewModel.PhoneNumber))
            {

                ModelState.AddModelError("PhoneNumber", "رقم الهاتف موجود بالفعل");

                return View(companyViewModel);
            }

            var code = TempData["_code"] as string ?? "";
            if (code.Trim() != companyViewModel.VerificationCode)
            {
                ModelState.AddModelError("VerificationCode", "الكود غير صحيح. يرجى المحاولة مرة أخرى.");
                return View(companyViewModel);
            }

            ReturnDataFile fileuplaod = null;
            FileUploads newFileUpload = null;
            Console.WriteLine($"is comapny or shop {companyViewModel.IsComanyOrShop}");
            if (companyViewModel.IsComanyOrShop)
            {
                if (companyViewModel.CompanyRecord == null || companyViewModel.CompanyRecord.Length == 0)
                {
                    ModelState.AddModelError("CompanyRecord", "من فضلك ادخل السجل التجاري");

                    return View(companyViewModel);
                }
                Console.WriteLine("Enter in jfdsdls");
                try
                {
                    fileuplaod= await _uploadFileService.UploadFile(companyViewModel.CompanyRecord, "Records", 5);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    TempData["ErrorMessage"] = $"حدث خطأ أثناء رفع الملف: {ex.Message}";
                    return View(companyViewModel);
                }
            }

            // إنشاء المستخدم
            var user = _mapper.Map<ApplicationUser>(companyViewModel);
            user.EmailConfirmed = true;

            var role = "Company";
            var result = await AuthRepository.RegisterMethode(user, companyViewModel.Password, role);

            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "فشل التسجيل!";
                return View(companyViewModel);
            }

            if (companyViewModel.IsComanyOrShop && fileuplaod!=null)
            {
                newFileUpload = new FileUploads()
                {
                    FileName = fileuplaod.FileName,
                    FilePath = fileuplaod.FilePath,
                    FileType = Path.GetExtension(companyViewModel.CompanyRecord.FileName),
                    UploadedDate = DateTime.UtcNow,
                };

                unitOfWork.FileUploads.Create(newFileUpload);
                await unitOfWork.SaveChangesAsync();
            }

            // إنشاء حساب الشركة مع FileId فقط
            var companyAccount = new ComapnyAccount()
            {
                Location = companyViewModel.Location,
                IsComanyOrShop = companyViewModel.IsComanyOrShop,
                FileId = newFileUpload?.Id,
                User = user
            };

            unitOfWork.ComapnyAccount.Create(companyAccount);
            await unitOfWork.SaveChangesAsync();

            await transaction.CommitAsync();
            return RedirectToAction("Login", "Account");
        }


        public IActionResult RegisterPersonal()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Countries = countries;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterPersonal(PersonalViewModel registerviewmodel)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Countries = countries;
                    return View();

                }
                if (await unitOfWork.UserManager.FindByEmailAsync(registerviewmodel.Email) != null)
                {
                    TempData["ErrorMessage"] = "البريد الإلكتروني موجود بالفعل!";
                    return RedirectToAction("RegisterPersonal");
                }

                if (unitOfWork.UserManager.Users.FirstOrDefault(e => e.PhoneNumber == registerviewmodel.PhoneNumber) != null)
                {
                    TempData["ErrorMessage"] = "رقم الهاتف موجود بالفعل!";
                    return RedirectToAction("RegisterPersonal");
                }

                var code = TempData["_code"] as string ?? "";

                if (code.Trim() != registerviewmodel.VerificationCode)
                {
                    ModelState.AddModelError("VerificationCode", "الكود غير صحيح. يرجى المحاولة مرة أخرى.");
                    return RedirectToAction("Register");

                }

                var user = _mapper.Map<ApplicationUser>(registerviewmodel);
                user.EmailConfirmed = true;
                var role = registerviewmodel.IsMarketer ? "Markter" : "Normal";
                var result = await AuthRepository.RegisterMethode(user, registerviewmodel.Password, role);

                if (result.Succeeded)
                {

                    if (role == "Markter")
                    {

                        var marketerAdded = await AuthRepository.AddLogicRoleToUserAsync(user, "Markter");

                        if (!marketerAdded)
                        {
                            await transaction.RollbackAsync();
                            TempData["ErrorMessage"] = "Failed to add marketer.";
                            return RedirectToAction("Register");
                        }

                        await transaction.CommitAsync();
                        return RedirectToAction("Login", "Account");
                    }

                    await transaction.CommitAsync();
                    return RedirectToAction("Login", "Account");

                }
                else
                {
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = "Registration failed";
                    return RedirectToAction("Register");

                }


            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                throw new Exception($"Error in RegisterPage {ex.Message}");

            }

        }
        public IActionResult Suggestion()
        {
            return View();
        }
        [HttpPost("/Account/verifyEmail")]
        public async Task<IActionResult> SendCodeToEmail([FromBody] EmailRequest emailRequest)
        {

            _code = GenrateCode();

            TempData["_code"] = _code;
            if (string.IsNullOrEmpty(emailRequest.Email))
                return BadRequest(new { message = "مشكله في ارسال الكود" });

            //await _emailSender
            //.SendEmailAsync(emailRequest.Email,
            //"verification code", $"<h3>{_code}</h3>");

            Console.WriteLine($"Code To User {_code}");

            return Ok(new { message = "تم ارسال الكود بنجاح" });


        }
        private string GenrateCode()
        {
            Random rn = new Random();
            return rn.Next(10000, 100000).ToString();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AuthGoogle(string returnUrl = "/")
        {

            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        //public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        //{
        //    var result = await HttpContext
        //        .AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //    if (!result.Succeeded)
        //    {
        //        return Challenge(GoogleDefaults.AuthenticationScheme);
        //    }

        //    var claims = result.Principal?.Identities?.FirstOrDefault()?.Claims?.Select(claim => new
        //    {
        //        claim.Type,
        //        claim.Value,
        //        claim.Issuer,
        //        claim.OriginalIssuer,
        //    });

        //    var emailClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";

        //    if (string.IsNullOrEmpty(emailClaim))
        //    {
        //        TempData["ErrorMessage"] = "لم يتم العثور على البريد الإلكتروني.";
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var user = await unitOfWork.UserManager.FindByEmailAsync(emailClaim);

        //    if (user == null)
        //    {
        //        TempData["ErrorMessage"] = "عفوا، يرجى إنشاء حساب جديد.";
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        return RedirectToAction("Register", "Account");
        //    }

        //    var roles = await unitOfWork.UserManager.GetRolesAsync(user);
        //    if (roles == null || roles.Count == 0)
        //    {
        //        TempData["ErrorMessage"] = "لم يتم العثور على أدوار للمستخدم.";
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var claimsUser = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Role, roles[0]),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //        new Claim(ClaimTypes.Name, user.FullName),
        //    };

        //    var claimsIdentity = new ClaimsIdentity(claimsUser, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        //    await _signInManager.SignInAsync(user, isPersistent: false);

        //    return RedirectToAction("Index", "Home");
        //}

        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ErrorMessage"] = "فشل الحصول على معلومات تسجيل الدخول الخارجي.";
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.
                ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على البريد الإلكتروني.";
                    return RedirectToAction(nameof(Login));
                }

                var user = await unitOfWork.UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var linkResult = await unitOfWork.UserManager.AddLoginAsync(user, info);
                    if (linkResult.Succeeded)
                    {

                        if (await unitOfWork.UserManager.IsInRoleAsync(user,"Admin"))
                        {
                            returnUrl = "/Admin/Index";
                        }
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "فشل ربط تسجيل الدخول الخارجي بالحساب.";
                        return RedirectToAction(nameof(Login));
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "لم يتم العثور على حساب مرتبط بهذا البريد الإلكتروني. يرجى إنشاء حساب جديد.";
                    return RedirectToAction("Login", new { ReturnUrl = returnUrl });
                }
            }
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost("/Account/ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (ModelState.IsValid)
            {


                var user = await unitOfWork.UserManager.FindByEmailAsync(forgotPasswordViewModel.Email);

                if (user == null)
                {
                    return BadRequest(new { Message = "هذا الايميل غير موجود" });
                }

                var code = await unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);

                var CallBackUrl = Url.Action("ResetPassword", "Account",
                    new { token = code, email = forgotPasswordViewModel.Email }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(forgotPasswordViewModel.Email, "Link To Reset Your Password",
                    $"<a href='{CallBackUrl}'>إعادة تعيين كلمة المرور</a>", "لينك اعاده تعيين كلمه المررورالخاصه بك");

                return Ok(new { Message = "تم ارسال لينك اعاده تعيين البياسورد" });


            }

            return BadRequest(new { Message = "برجاء ادخال ايميل صحيح" });
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            Console.WriteLine(token);
            Console.WriteLine(email);
            if (token == null || email == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                Console.WriteLine(model.Email);
                Console.WriteLine(model.Token);
                Console.WriteLine("ebter enter");
                return View(model);
            }
            var user = await unitOfWork.UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "هذا المستخدم غير موجود";
                return RedirectToAction("Login", "Account");
            }
            var result = await unitOfWork.UserManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {

                TempData["SuccessMessage"] = "تم تغير كلمه السر بنجاح";

                return RedirectToAction("Login", "Account");
            }
            string message = "";
            foreach (var error in result.Errors)
            {
                message += error.Description;
            }
            TempData["MessageReset"] = message;
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            var model = new AccessDeniedViewModel
            {
                Message = "You do not have permission to access this page.",
                StatusMessage = "Access Denied - 403"
            };

            return View(model);
        }


    }

}
