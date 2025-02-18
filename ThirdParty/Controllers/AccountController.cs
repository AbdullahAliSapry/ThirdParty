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
        public AccountController(ISendEmail emailSender, IUnitOfWork<ApplicationUser> unitOfWork, IAuthRepository authRepository, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            countries = CountryRepository.Countries;
            _code = string.Empty;
            _emailSender = emailSender;
            this.unitOfWork = unitOfWork;
            AuthRepository = authRepository;
            _mapper = mapper;
            this._signInManager = signInManager;
        }


        [HttpGet("/Account/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            Console.WriteLine("Enter login funcation");

            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            var user = await AuthRepository.LoginMethode(loginVm);

            if (user == null)
            {
                TempData["ErrorMessage"] = "خطأ في الايميل او كلمه المرور";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, true, false);

            if (result.Succeeded)
            {


                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var role = await unitOfWork.UserManager.GetRolesAsync(user);
          
                var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, "hello "),
                                new Claim(ClaimTypes.Name, user.FullName.ToString()),
                                new Claim(ClaimTypes.Email,user.Email.ToString()),
                                new Claim(ClaimTypes.Role,role[0]),
                            };


                var claimsIdentity =
                    new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var claimsPrincipal =
                    new ClaimsPrincipal(claimsIdentity);

                await HttpContext.
                    SignInAsync(CookieAuthenticationDefaults.
                    AuthenticationScheme, claimsPrincipal);

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "فشل في تسجيل الدخول.");
                    return View(loginVm);
                }
            }
            else
            {
                // في حال فشل تسجيل الدخول
                ModelState.AddModelError(string.Empty, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
                return View(loginVm);
            }
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
                    return RedirectToAction("Register");
                }

                if (unitOfWork.UserManager.Users.FirstOrDefault(e => e.PhoneNumber == registerviewmodel.PhoneNumber) != null)
                {
                    TempData["ErrorMessage"] = "رقم الهاتف موجود بالفعل!";
                    return RedirectToAction("Register");
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
                await AuthRepository.RegisterMethode(user, registerviewmodel.Password, role);
                await transaction.CommitAsync();
                return RedirectToAction("Login", "Account");

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

            await _emailSender
            .SendEmailAsync(emailRequest.Email, "verification code", $"<h3>{_code}</h3>");

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

            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public async Task AuthGoogle()
        {

            await HttpContext.
                ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext
                .AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return Challenge(GoogleDefaults.AuthenticationScheme);
            }

            var claims = result.Principal?.Identities?.FirstOrDefault()?.Claims?.Select(claim => new
            {
                claim.Type,
                claim.Value,
                claim.Issuer,
                claim.OriginalIssuer,
            });

            var emailClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";

            if (string.IsNullOrEmpty(emailClaim))
            {
                TempData["ErrorMessage"] = "لم يتم العثور على البريد الإلكتروني.";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }

            var user = await unitOfWork.UserManager.FindByEmailAsync(emailClaim);

            if (user == null)
            {
                TempData["ErrorMessage"] = "عفوا، يرجى إنشاء حساب جديد.";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Register", "Account");
            }

            var roles = await unitOfWork.UserManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
            {
                TempData["ErrorMessage"] = "لم يتم العثور على أدوار للمستخدم.";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }

            var claimsUser = new List<Claim>
            {
                new Claim(ClaimTypes.Role, roles[0]),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
            };

            var claimsIdentity = new ClaimsIdentity(claimsUser, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
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




    }

}
