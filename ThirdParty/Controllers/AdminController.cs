using AutoMapper;
using Bll.Dtos;
using Bll.Dtos.ApiDto;
using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Xml;
using static StackExchange.Redis.Role;

namespace ThirdParty.Controllers
{
    [Authorize(Roles = "Admin,SubAdmin")]
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;

        private IUnitOfWork<Order> _unitOfWork { get; set; }
        private IMapper _mapper;

        public AdminController(ILogger<AdminController> logger, IUnitOfWork<Order> unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            var ordersReviewd = _unitOfWork.Order.GetItemsWithFunc(
                              e => true,
                              new string[] { "CartItems" }
                          ).OrderBy(e => e.IsPrivewed == status).ToList();


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

            if ( proplemId < 0)
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
    }


}
