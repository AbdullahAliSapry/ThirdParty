using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ThirdParty.Controllers
{
    public class RecaptchaController : Controller
    {
        private readonly IConfiguration _configuration;
        private IUnitOfWork<BannedIP> _unitOfWork { get; set; }
        public RecaptchaController(IConfiguration configuration, IUnitOfWork<BannedIP> unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyRecaptcha([FromBody] RecaptchaRequest request)
        {
            Console.WriteLine("Recaptcha Token: " + request.Token);
            var secretKey = _configuration["Recaptcha:SecretKey"];
            var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            Console.WriteLine("User IP: " + ipAddress);

            var client = new HttpClient();
            var values = new Dictionary<string, string>
                {
                    { "secret", secretKey },
                    { "response", request.Token }
                };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var responseString = await response.Content.ReadAsStringAsync();


            var recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(responseString);

            // التحقق من النجاح ودرجة الأمان
            if (recaptchaResponse.Success)
            {
                Console.WriteLine("Recaptcha Success");
                if (recaptchaResponse.Score < 0.5)
                {

                    var newBan = new BannedIP
                    {
                        IPAddress = ipAddress,
                        FailureCount = 1,
                        LastFailed = DateTime.Now,
                        BanUntil = DateTime.Now.AddDays(50)
                    };

                    _unitOfWork.BannedIP.Create(newBan);
                    _unitOfWork.SaveChanges();
                    Console.WriteLine("Failed due to low score");
                    return StatusCode(403, "Forbidden: Low score on Recaptcha"); 
                    
                }
                else
                {
                    Console.WriteLine("Passed successfully");
                    return Ok(); 
                }
            }
            else
            {
                Console.WriteLine("Failed: Recaptcha unsuccessful");
                return Forbid(); // الفشل في التحقق
            }
        }
        public class RecaptchaRequest
        {
            public string Token { get; set; }
        }

        public class RecaptchaResponse
        {
            public bool Success { get; set; }
            public float Score { get; set; }
        }

    }
}

