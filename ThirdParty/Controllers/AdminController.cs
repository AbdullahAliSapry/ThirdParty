using Microsoft.AspNetCore.Mvc;

namespace ThirdParty.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
