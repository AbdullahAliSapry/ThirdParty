using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThirdParty.ViewModels;

namespace ThirdParty.Controllers
{

    public class UserSettingController : Controller
    {
        [Authorize]
        public async Task<IActionResult> SettingUser(string userid)
        {
            Console.WriteLine("Ebter");
       

            if (string.IsNullOrEmpty(userid) || !Guid.TryParse(userid, out Guid ValidId))
            {
                var error = new ErrorViewModel
                {
                    RequestId = "400",
                    Message = "Invalid User Id. Please ensure the Id is correct."
                };
                return View("Error", error);
            }

            var name = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var Name = User.FindFirst(ClaimTypes.Name)?.Value;


            Console.WriteLine(Name);
            Console.WriteLine(email);
            Console.WriteLine(name);

            return View();
        }
    }
}
