using AutoMapper;
using Bll.Dtos;
using Bll.Dtos.ApiDto;
using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Threading.Tasks;
using ThirdParty.ViewModels;

namespace ThirdParty.Controllers
{

    [Authorize]
    public class CartController : Controller
    {

        private readonly ILogger<CartController> _logger;
        private IUnitOfWork<Cart> _unitOfWork { get; set; }

        private readonly IMapper _mapper;
        public CartController(ILogger<CartController> logger, IUnitOfWork<Cart> unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [Authorize]
        public async Task<IActionResult> Index(string userId)
        {
            var checkauth = User.FindFirstValue(ClaimTypes.NameIdentifier) == userId ? true : false;


            if (!checkauth)
            {
                return Forbid();
            }

            var cart = await _unitOfWork.Cart.GetItemWithFunc(e => e.UserId == userId, new string[] { "CartItems.AttributeItems", "CartItems.Category" });
            if (cart == null)
            {
                cart = new Cart() { UserId = userId };
                _unitOfWork.Cart.Create(cart);
                _unitOfWork.SaveChanges();
            }
            var AllTax = _unitOfWork.CommissionScheme
                .GetItemsWithFunc(e => e.IsActive && e.UserType == UserType.Normal);
            Console.WriteLine($"totola tax count {AllTax.Count}");
            ViewData["AllTax"] = AllTax;
            cart.CartItems = cart.CartItems.Where(e => !e.IsOredered).ToList();

            var cartdto = _mapper.Map<CartDto>(cart);
            return View(cartdto);
        }

        [HttpPost("/Cart/AddItemInCart/{userId}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItemInCart([FromBody] CartItemDto cartItem, string userId)
        {

            try
            {
                Console.WriteLine("Enter in added");
                // check on user if aurgrize 
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var isauth = User.FindFirstValue(ClaimTypes.NameIdentifier) == userId ? true : false;
                if (!isauth)
                    return StatusCode(403, new { Message = "Acess Denied Please Login", Success = false });

                // get cart user

                var cart = await _unitOfWork.Cart.GetItemWithFunc(e => e.UserId == userId, new string[] { "CartItems" });

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };

                    _unitOfWork.Cart.Create(cart);

                    _unitOfWork.SaveChanges();

                }

                // check is order is null
                var checkitermExitstincart = cart.CartItems.FirstOrDefault(e => e.ProductId == cartItem.ProductId && !e.IsOredered);


                if (checkitermExitstincart != null)
                {
                    return BadRequest(new { Message = "Item Aready Exist In Cart", success = false });

                }


                var itemcart = _mapper.Map<CartItem>(cartItem);

                cart.CartItems.Add(itemcart);

                _unitOfWork.Cart.UpdateItem(cart);
                _unitOfWork.SaveChanges();


                return Ok(new { Message = "Added To Cart Succes", success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { Message = $"failed To Add Cart" });
            }



        }


        [HttpPatch("/Cart/UpdateQuntity/{userId}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateQuntity([FromBody] UpdateQuntityInfoDto UpdateQuntityInfo, string userId)
        {
            _logger.LogInformation("enter in methode success");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var checkauth = User.FindFirstValue(ClaimTypes.NameIdentifier) == userId;

            if (!checkauth)
                return StatusCode(403, new { Message = "Access Denied. Please check your ID" });

            var cart = await _unitOfWork.Cart.GetItemWithFunc(e => e.UserId == userId, new string[] { "CartItems.AttributeItems" });

            if (cart.CartItems.Count == 0)
                return BadRequest(new { Message = "Your Cart is Empty" });

            var cartItem = cart.CartItems.FirstOrDefault(e => e.Id == UpdateQuntityInfo.CartitemId);

            if (cartItem == null)
                return NotFound(new { Message = "Cart Item Not Found" });

            cartItem.Quntity = UpdateQuntityInfo.newquntity;

            if (UpdateQuntityInfo.isquntityAttrbute)
            {
                if (UpdateQuntityInfo.ItemAttrbuteId == null || UpdateQuntityInfo.ItemAttrbuteId == 0)
                    return BadRequest(new { Message = "Your item attribute ID is required" });

                var attrbuteitem = cartItem.AttributeItems.FirstOrDefault(e => e.Id == UpdateQuntityInfo.ItemAttrbuteId);

                if (attrbuteitem == null)
                    return NotFound(new { Message = "Attribute item not found" });

                attrbuteitem.Quantity = UpdateQuntityInfo.newquntity;
            }

            var result = _unitOfWork.CartItem.UpdateItem(cartItem);

            if (result)
            {
                _unitOfWork.SaveChanges();
                return Ok(new { Message = "Quantity Updated Successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed to Update Quantity" });
            }
        }

        [Authorize]
        public async Task<IActionResult> DeleteItem(string userId, int cartitmemid, string cartId)
        {

            var checkauth = User.FindFirstValue(ClaimTypes.NameIdentifier) == userId;

            if (!checkauth)
                return View("AccessDenied", new AccessDeniedViewModel { Message = "Access Denied. Please check your ID", StatusMessage = "403" });

            if (!Guid.TryParse(cartId, out Guid CartId))
                return View("Error", new ErrorViewModel { Message = "Cart Not Found", RequestId = "404" });


            var cart = await _unitOfWork.Cart.GetItemWithFunc(e => e.Id == CartId && e.UserId == userId, new string[] { "CartItems" });

            if (cart == null)
                return View("Error", new ErrorViewModel { Message = "Cart Not Found", RequestId = "404" });

            var cartitem = cart.CartItems.Where(e => e.Id == cartitmemid).FirstOrDefault();

            if (cartitem == null)
            {
                TempData["ErrorMessage"] = "هذا العنصر غير موجود";
                return RedirectToAction("Index", "Cart", new { userId });
            }


            var result = await _unitOfWork.CartItem.DeleteItemWithFunc(e => e.Id == cartitem.Id);


            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم حذف العنصر بنجاح";
                return RedirectToAction("Index", "Cart", new { userId });
            }
            else
            {
                TempData["ErrorMessage"] = "فشل حذف العنصر";
                return RedirectToAction("Index", "Cart", new { userId });
            }
        }

    }
}
