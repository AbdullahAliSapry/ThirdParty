using AutoMapper;
using Bll.Dtos;
using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using Infrastructure.ApiotService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using ThirdParty.ViewModels;
using Twilio.Rest.Trunking.V1;
using static ThirdParty.Controllers.OrderController;

namespace ThirdParty.Controllers
{

    [Authorize]
    public class OrderController : Controller
    {

        private IUnitOfWork<Order> _unitOfWork { get; set; }

        private readonly IMapper _mapper;

        private ILogger<OrderController> _logger;

        private readonly IMemoryCache _memoryCache;
        private IApiService<object> _apiServiceToProduct;




        public OrderController(IUnitOfWork<Order> unitOfWork, IMapper mapper, ILogger<OrderController> logger, IMemoryCache memoryCache, IApiService<object> apiServiceToProduct)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
            _apiServiceToProduct = apiServiceToProduct;
        }


        [Authorize]
        public async Task<IActionResult> Index(string userId)
        {
            // getuser
            if (userId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

            var cart = await _unitOfWork.Cart
                      .GetItemWithFunc(e => e.UserId == userId, new string[] { "CartItems" });

            if (cart == null || cart.CartItems == null)
            {

                return View();

            }

            var cartitems = cart.CartItems.Where(e => e.IsOredered).ToList();

            if (!cartitems.Any())
            {
                return View();
            }

            var orderIds = cartitems.Select(c => c.OrderId).Distinct().ToList();
            var orders = _unitOfWork.Order.GetItemsWithFunc(e => orderIds.Contains(e.Id));

            var groupedOrders = new OrderesView
            {
                OrderUnderReview = orders.Where(o => !o.IsPrivewed && !o.IsAccesepted).ToList(),

                // الطلبات التي تم مراجعتها وتم قبولها لكنها لم تنتهِ بعد
                OrderReviewd = orders.Where(o => o.IsPrivewed && !o.StatusOrder).ToList(),

                // الطلبات التي تم مراجعتها وتم قبولها وانتهت
                OrderFinsed = orders.Where(o => o.IsPrivewed && o.IsAccesepted && o.StatusOrder).ToList(),

                // الطلبات التي تم مراجعتها ولكن تم رفضها
                Rejected = orders.Where(o => o.IsPrivewed && !o.IsAccesepted).ToList()
            };


            return View(groupedOrders);
        }


        public class OrderesView
        {
            public List<Order> OrderUnderReview { get; set; } = new List<Order>();
            public List<Order> OrderReviewd { get; set; } = new List<Order>();
            public List<Order> OrderFinsed { get; set; } = new List<Order>();
            public List<Order> Rejected { get; set; } = new List<Order>();
        }



        [Authorize]
        [HttpPost("/Order/AddOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderData addOrderData)
        {
            // التحقق من هوية المستخدم
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != addOrderData.UserId)
                return StatusCode(403, new { Message = "Access Denied", success = false });

            // جلب سلة التسوق للمستخدم
            var cart = await _unitOfWork.Cart
                .GetItemWithFunc(e => e.UserId == addOrderData.UserId, new string[] { "CartItems" });

            if (cart?.CartItems == null || cart.CartItems.Count == 0)
                return BadRequest(new { Message = "Cart is empty", success = false });

            // التحقق من صحة عناصر السلة المطلوبة
            var cartitemRequires = cart.CartItems
                .Where(c => !c.IsOredered && addOrderData.IdsCartItems.Contains(c.Id))
                .ToList();

            if (cartitemRequires.Count != addOrderData.IdsCartItems.Count)
                return BadRequest(new { Message = "Invalid order products", success = false });

            // جلب بيانات المنتجات من الكاش أو الخدمة الخارجية
            var products = new List<ProductDto>();
            var notFoundProductsIds = new List<string>();

            foreach (var cartitem in cartitemRequires)
            {
                string cacheKeyToProduct = $"Product__{cartitem.ProductId}__{CultureInfo.CurrentCulture.Name}";

                if (_memoryCache.TryGetValue(cacheKeyToProduct, out ProductDetailsViewModel cachedProductVm))
                    products.Add(cachedProductVm.Product);
                else
                    notFoundProductsIds.Add(cartitem.ProductId);
            }

            // جلب المنتجات غير الموجودة في الكاش من الخدمة الخارجية
            if (notFoundProductsIds.Any())
            {
                var Params = new Dictionary<string, string>
                                {
                                    { "idsList", string.Join(";", notFoundProductsIds) },
                                    { "language", CultureInfo.CurrentCulture.Name },
                                    { "blockList", "" }
                                };

                dynamic response = await _apiServiceToProduct.GetDataAsyncDynmic("GetItemInfoList", Params);

                if (response == null)
                    return BadRequest(new { Message = "❌ Failed to validate data", success = false });

                var myData = JsonConvert.DeserializeObject<ApiResponseDtoFour<ProductDto>>(
                    JsonConvert.SerializeObject(response)
                );

                if (myData?.OtapiItemInfoList?.Content == null)
                    return BadRequest(new { Message = "❌ Failed to validate data", success = false });

                products.AddRange(myData.OtapiItemInfoList.Content);
            }

            if (products.Count != addOrderData.IdsCartItems.Count)
                return BadRequest(new { Message = "Some products not found", success = false });

            double totalPriceToOrder = 0.0, totaltax = 0.0, TotalTaxWithOutMarkerDiscount = 0.0;

            foreach (var product in products)
            {
                var item = cartitemRequires.FirstOrDefault(e => e.ProductId == product.Id);

                if (item == null)
                    return BadRequest(new { Message = "Product mismatch", success = false });

                double itemTotalPrice = (double)product.Price.ConvertedPriceList.Internal.Price * item.Quntity;
                totalPriceToOrder += itemTotalPrice;

                if (item.PricePerPiece * item.Quntity != itemTotalPrice)
                    return BadRequest(new { Message = "Order data mismatch", success = false });
            }

            // to duble value
            if (Math.Ceiling(totalPriceToOrder) != Math.Ceiling(addOrderData.TotoalPrice))
                return BadRequest(new { Message = "Order total mismatch", success = false });

            // حساب الضريبة بناءً على نوع المستخدم
            var userType = User.FindFirstValue(ClaimTypes.Role) == "Markter" ? UserType.Markter : UserType.Normal;
            var commisions = _unitOfWork.CommissionScheme.GetItemsWithFunc(e => e.IsActive && e.UserType == userType);
            var commisionRequired = new CommissionScheme();
            foreach (var commision in commisions)
            {
                if (totalPriceToOrder > commision.LowerLimit && totalPriceToOrder <= commision.UpperLimit)
                {
                    totaltax = totalPriceToOrder * commision.CommissionRate;
                    commisionRequired = commision;
                    break;
                }
            }

            //  
            Console.WriteLine($"totoal tex is {totaltax}");
            if (Math.Ceiling(totaltax) != Math.Ceiling(addOrderData.TotolaTax))
                return BadRequest(new { Message = "Tax mismatch", success = false });

            TotalTaxWithOutMarkerDiscount = totaltax;
            // تطبيق كود الخصم
            var code = new CodesToMarketer();
            if (!string.IsNullOrEmpty(addOrderData.Code))
            {
                code = await _unitOfWork.CodeToMarketer.GetItemWithFunc(e => e.Code == addOrderData.Code);
                if (code?.DiscountRate > 0)
                {
                    var commisionMatkter = _unitOfWork.
                        CommissionScheme
                        .GetItemsWithFunc(e => e.IsActive && e.UserType ==
                        UserType.Markter && e.UpperLimit > totalPriceToOrder && e.LowerLimit < totalPriceToOrder).FirstOrDefault();


                    double discountAmount = totalPriceToOrder * ((double)code.DiscountRate / 100) * commisionMatkter.CommissionRate;
                    Console.WriteLine($"discountAmount {discountAmount}");
                    totaltax -= discountAmount;
                    Console.WriteLine($"value 1 {addOrderData.TotoalPriceWithTax} value2 {totalPriceToOrder + totaltax}");
                    if (Math.Ceiling(addOrderData.TotoalPriceWithTax) != Math.Ceiling((totalPriceToOrder + totaltax)))
                        return BadRequest(new { Message = "Discount mismatch", success = false });
                }
            }

            // إنشاء الطلب
            var order = new Order
            {
                CartItems = cartitemRequires,
                CreatedAt = DateTime.UtcNow,
                IsPrivewed = false,
                IsAccesepted = false,
                StatusOrder = false,
                ShippingPrice = 0.0,
                Tax = totaltax,
                TotalPrice = totalPriceToOrder,
                UpdaetdAt = DateTime.UtcNow,
                CommentOnOrder = "No Comment Now",
                TotalTaxWithOutMarkerDiscount = TotalTaxWithOutMarkerDiscount,
                Link1688 = string.Join(",", products.Select(e => e.ExternalItemUrl)),
            };

            var result = _unitOfWork.Order.Create(order);

            if (!result.Status)
                return BadRequest(new { Message = "Failed to add order", success = false });

            _unitOfWork.SaveChanges();

            // تحديث عناصر السلة إلى "تم الطلب"
            cartitemRequires.ForEach(e => e.IsOredered = true);
            var updatedResult = await _unitOfWork.CartItem.UpdateAsync(cartitemRequires);

            if (updatedResult != null)
                await _unitOfWork.SaveChangesAsync();

            // تسجيل استخدام كود الإحالة
            if (!string.IsNullOrEmpty(addOrderData.Code))
            {
                var user = await _unitOfWork.User.GetItemWithFunc(e => e.Id == addOrderData.UserId);
                if (user == null)
                    return NotFound(new { Message = "User not found", success = false });

                order.ReferralCodeUsage = new ReferralCodeUsage
                {
                    ApplicationUser = user,
                    CodesToMarketer = code,
                };

                var referralResult = _unitOfWork.Order.UpdateItem(order);
                if (!referralResult)
                    return BadRequest(new { Message = "Failed to apply referral code", success = false });

                _unitOfWork.SaveChanges();
            }

            return Json(new { Message = "Order added successfully", success = true });
        }

        public class AddOrderData
        {
            public string UserId { get; set; } = null!;
            public List<int> IdsCartItems { get; set; } = null!;
            public string? Code { get; set; }
            public double TotoalPrice { get; set; }
            public double TotolaTax { get; set; }
            public int TotalQunity { get; set; }
            public double TotoalPriceWithTax { get; set; }
        }




        [Authorize]
        [HttpPatch("/Order/ChangeStatusOreder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatusOreder(string userId, string orderId, bool status)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
                return StatusCode(403, new { Message = "Access Denied", success = false });

            if (!Guid.TryParse(orderId, out Guid OrderId))
                return BadRequest(new { Message = "InValid Id " });


            var order = await _unitOfWork.Order.GetItemWithFunc(e => e.Id == OrderId);
            if (!order.IsPrivewed)
                return BadRequest(new { Message = "Order Not Reviewd" });


            order.IsAccesepted = status;
            _unitOfWork.Order.UpdateItem(order);
            _unitOfWork.SaveChanges();


            return Json(new { Message = "Status Changes Success", Status = status });

        }

    }
}
