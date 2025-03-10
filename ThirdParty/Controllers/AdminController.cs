using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.Tasks;

namespace ThirdParty.Controllers
{
    [Authorize(Roles = "Admin,SubAdmin")]
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;

        private IUnitOfWork<Order> _unitOfWork { get; set; }

        public AdminController(ILogger<AdminController> logger, IUnitOfWork<Order> unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {


            // order not reviewd
            var orders = _unitOfWork.Order.GetItemsWithFunc(e => !e.IsPrivewed,new string[] { "ReferralCodeUsage" });

            var users = _unitOfWork.User.GetItems();
            // payments

            var paymentsnotactive = _unitOfWork.PayMentManoul.GetItemsWithFunc(e => !e.IsConfirmed);

            // proplems

            var proplems = _unitOfWork.Proplem.GetItemsWithFunc(e=>!e.IsSolved);

            ViewData["Orders"] = orders;
            ViewData["Users"]= users;
            ViewData["Payemnts"]= paymentsnotactive;
            ViewData["Proplems"] = proplems;


            _logger.LogInformation("Data Uploaded Succesfully");

            return View();
        }
        
        public IActionResult AllUsers()
        {
           
            var users = _unitOfWork.User.GetItems();

            return View(users);
        }


    }
}
