using Microsoft.AspNetCore.Mvc;

namespace ThirdParty.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
