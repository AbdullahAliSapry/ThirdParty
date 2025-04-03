using ThirdParty.ViewModels;
using Bll.Dtos;
using Contracts.SharedDtos;
using Infrastructure.ApiotService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Globalization;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;
using DAl.IRepository;
using DAl.Models;
using DAl.Repository;
using static ThirdParty.Controllers.ProductController;
using ThirdParty.Utilities;
using Twilio.TwiML.Messaging;
using Twilio.Annotations;
using ThirdParty.Views.Shared.Components.FavoriteSallerCom;
using Infrastructure.FileUploadService.FileUploadOnService;

namespace ThirdParty.Controllers
{
    public class ProductController : Controller
    {

        private IApiService<object> _apiServiceToProduct;
        private readonly IMemoryCache _memoryCache;
        private IUnitOfWork<Favorite> unitOfWork { get; set; }
        private IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private IFileUploadUloudinary _fileUploadUloudinary { get; }

        public ProductController(IApiService<object> apiService, IMemoryCache memoryCache, IMapper mapper, IUnitOfWork<Favorite> unitOfWork, ILogger<ProductController> logger, IFileUploadUloudinary fileUploadUloudinary)
        {
            _apiServiceToProduct = apiService;
            _memoryCache = memoryCache;
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
            _logger = logger;
            _fileUploadUloudinary = fileUploadUloudinary;
        }

        public async Task<IActionResult> ProductsToCategory(
            string categoryName,
            string categoryId,
            string subcategoryId = "",
            string subcategoryName = "",
            string searchTerm = "",
            int page = 1,
            int pageSize = 20,
            double? minPrice = null,      // ✅ Added
            double? maxPrice = null       // ✅ Added
        )
        {
            string cacheKey = $"Products_{categoryId}_{page}_{searchTerm}_{subcategoryId}_{minPrice}_{maxPrice}_{CultureInfo.CurrentCulture.Name.Trim()}";
            int totalItems = 0;
            int totalPages = 0;

            if (!_memoryCache.TryGetValue(cacheKey, out ProductsCacheDto cacheData))
            {
                Console.WriteLine("Fetching data from API...");
                double min = 1.0;
                var max = 0.0;
                if (minPrice != null && minPrice != 0 && maxPrice != null && maxPrice != 0)
                {
                    min = Math.Ceiling(((double)minPrice / 3.75));
                    max =Math.Ceiling((double)maxPrice / 3.75);
                }

                var searchParams = new SearchItemsParameters
                {
                    CategoryId = !string.IsNullOrEmpty(subcategoryId) ? subcategoryId : categoryId,
                    ItemTitle = !string.IsNullOrEmpty(searchTerm) ? searchTerm : "",
                    MinPrice = min >= 0 ?
                    min.ToString() : "",
                    MaxPrice = max >= 0 ? max.ToString() : "",
                    LanguageOfQuery = CultureInfo.CurrentCulture.Name == "ar" ? "ar" : "en"
                };

                var xmlParams = XmlHelper.ConvertToXml(searchParams);

                var apiParams = new Dictionary<string, string>{
                                        { "framePosition", ((page - 1) * pageSize).ToString() },
                                        { "frameSize", pageSize.ToString() },
                                        { "xmlParameters", xmlParams },
                                        { "blockList", "" },
                                        { "signature", "" },
                                        { "timestamp", "" },
                                        { "language", CultureInfo.CurrentCulture.Name }
                                    };

                dynamic response = await _apiServiceToProduct.GetDataAsyncDynmic("BatchSearchItemsFrame", apiParams);
                string jsonString = JsonConvert.SerializeObject(response);
                var myData = JsonConvert.DeserializeObject<ApiResponseDtoTow<ProductDto>>(jsonString);

                if (myData?.Result?.Items?.Items?.Content != null)
                {


                    var products = myData.Result.Items.Items.Content;
                    totalItems = myData.Result.Items.Items.TotalCount;
                    totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);



                    cacheData = new ProductsCacheDto
                    {
                        Products = products,
                        TotalPages = totalPages
                    };

                    // handle converted price 


                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                        Size = 1024
                    };

                    _memoryCache.Set(cacheKey, cacheData, cacheOptions);
                }
                else
                {
                    Console.WriteLine("API returned empty data!");
                    return View(new ProductsViewModel());
                }
            }
            else
            {
                Console.WriteLine("Data found in cache");
            }

            var subcategory = unitOfWork.Category.GetItemsWithFunc(e => e.ParentId == categoryId);
            ViewBag.Categories = _mapper.Map<List<CategoryDto>>(subcategory);

            var viewModel = new ProductsViewModel
            {
                Products = cacheData.Products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = cacheData.TotalPages
            };

            return View(viewModel);
        }

        // this funcation contani logic error
        //VendorDto
        public async Task<IActionResult> ProductDetails(string productId, string categoryId, string categoryName, int page)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return View("Error", new ErrorViewModel { Message = "❌ Product ID is required.", RequestId = "400" });
            }

            //string cacheKeyToProducts = $"Products_{categoryId}_{page + 1}_{CultureInfo.CurrentCulture.Name.Trim()}";
            string cacheKeyToProduct = $"Product__{productId}__{CultureInfo.CurrentCulture.Name}";

            var productVm = new ProductDetailsViewModel
            {
                Product = new ProductDto(),
                Vendor = new VendorDto(),
                VendorItems = new VendorItemDto(),
                Reviews = new ProviderReviews()
            };

            if (_memoryCache.TryGetValue(cacheKeyToProduct, out ProductDetailsViewModel cachedProductVm))
            {
                Console.WriteLine("✅ Product found in cache.");
                return View(cachedProductVm);
            }


            var Params = new Dictionary<string, string>
                            {
                                { "itemId", productId },
                                { "language", CultureInfo.CurrentCulture.Name },
                                { "blockList", "Vendor,ProviderReviews,MostPopularVendorItems16" }
                            };


            if (!_memoryCache.TryGetValue(cacheKeyToProduct, out ProductsCacheDto productsCacheDto))
            {
                Console.WriteLine("🌐 Fetching product data from API...");

                dynamic response = await _apiServiceToProduct.GetDataAsyncDynmic("BatchGetItemFullInfo", Params);

                if (response == null)
                {
                    return View("Error", new ErrorViewModel { Message = "❌ API response is null.", RequestId = "500" });
                }

                var jsonString = JsonConvert.SerializeObject(response);
                var myData = JsonConvert.DeserializeObject<ApiProductDeatilsDto<ProductDto,
                    VendorDto, VendorItemDto,
                    ProviderReviews>>(jsonString);

                if (myData == null)
                {
                    return View("Error", new ErrorViewModel { Message = "❌ Deserialized data is null.", RequestId = "500" });
                }

                if (myData.Result == null)
                {
                    return View("Error", new ErrorViewModel { Message = "❌ Result in API data is null.", RequestId = "500" });
                }

                if (myData.Result.Item == null || myData.Result.Vendor == null || myData.Result.VendorItems == null)
                {
                    return View("Error", new ErrorViewModel { Message = $"❌ Product with ID {productId} not found in API.", RequestId = "404" });
                }

                // ✅ تعيين القيم في ViewModel

                productVm.Product = myData.Result.Item;
                productVm.Vendor = myData.Result.Vendor;
                productVm.VendorItems = myData.Result.VendorItems;
                productVm.Reviews = myData.Result.ProviderReviews;

                // ✅ إضافة البيانات إلى الكاش
                _memoryCache.Set(cacheKeyToProduct, productVm, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Size = 1024
                });

                return View(productVm);
            }



            return View(productVm);
        }


        public class ProductsCacheDto
        {
            public List<ProductDto> Products { get; set; }
            public int TotalPages { get; set; }
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("/Product/AddProductToFavoutits")]
        public async Task<IActionResult> AddProductToFavorites([FromBody] UtlitiesAdding<FavoriteItem> productFav)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(productFav?.UserId);

            if (userId == null || productFav?.UserId != userId)
            {
                return Unauthorized(new { Message = "Invalid User Id" });
            }

            var user = await unitOfWork.User.GetItem(productFav.UserId, new string[] { "Favorite" });

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            if (user.Favorite == null)
            {
                user.Favorite = new Favorite { FavoriteItems = new List<FavoriteItem>() };
            }

            if (productFav?.Product == null)
            {
                return BadRequest(new { Message = "Invalid product data" });
            }

            var favcart = await unitOfWork.Favorite.GetItemWithFunc(e => e.UserId == user.Id, new string[] { "FavoriteItems" });
            if (user.Favorite.FavoriteItems != null && favcart.FavoriteItems.Any(e => e.ProductId == productFav.Product.ProductId))
            {
                Console.WriteLine("enter in check exitst");
                return BadRequest(new { Message = "Product already added" });
            }

            Console.WriteLine("Enter in end");

            var favorite = _mapper.Map<FavoriteItem>(productFav.Product);
            favorite.Quntity = productFav.Quntity;

            user.Favorite.FavoriteItems.Add(favorite);

            var updateResult = unitOfWork.User.UpdateItem(user);
            if (!updateResult)
            {
                return StatusCode(500, new { Message = "Error updating user favorites" });
            }

            var saveResult = await unitOfWork.SaveChangesAsync();
            if (!saveResult)
            {
                return StatusCode(500, new { Message = "Failed to save changes" });
            }

            return Ok(new { Message = "Added successfully" });
        }


        public class UtlitiesAdding<T> where T : class
        {
            public T Product { get; set; }

            public string UserId { get; set; }

            public int Quntity { get; set; }

        }


        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> Search(string title = "", int page = 0, int pageSize = 30, string fileUrl = "", string vendorid = "", string vendorName = "")
        {

            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(fileUrl) && string.IsNullOrEmpty(vendorid) && string.IsNullOrEmpty(vendorName))
            {
                Console.WriteLine("Enter in null valye");
                return View(new ProductsViewModel());
            }

            int totalItems = 0;
            int totalPages = 0;
            var lg = CultureInfo.CurrentCulture.Name == "ar" ? "ar" : "en";
            // conerting
            var Xmlparams = XmlHelper
                .ConvertToXml(new SearchItemsParameters
                {
                    ItemTitle = title,
                    VendorName
                = vendorName,
                    VendorId = vendorid,
                    LanguageOfQuery = lg
                });

            var cacheKey = $"Search_{title}_{page}_{pageSize}_{vendorName}_{vendorid}_{lg}";
            //if (!string.IsNullOrEmpty(fileUrl))
            //{
            //    Console.WriteLine(fileUrl);
            //    cacheKey += $"_{fileUrl}";

            //    Xmlparams =
            //       XmlHelper.ConvertToXml(new SearchItemsParameters { ItemTitle = title, ImageUrl = fileUrl });
            //}


            var Params = new Dictionary<string, string>
                        {
                            {"framePosition", page.ToString()},
                            {"frameSize", pageSize.ToString()},
                            {"xmlParameters", Xmlparams},
                            {"blockList","" },
                            {"signature", "" },
                            {"timestamp", "" },
                            {"language", CultureInfo.CurrentCulture.Name}
                        };

            if (!_memoryCache.TryGetValue(cacheKey, out ProductsCacheDto cacheData))
            {


                var response = await _apiServiceToProduct.GetDataAsyncDynmic("BatchSearchItemsFrame", Params);

                var datastring = JsonConvert.SerializeObject(response);

                var results = JsonConvert.DeserializeObject<ApiResponseDtoTow<ProductDto>>(datastring);

                if (results?.Result?.Items?.Items?.Content != null)
                {
                    var products = results.Result.Items.Items.Content;
                    totalItems = results.Result.Items.Items.TotalCount;
                    totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                    cacheData = new ProductsCacheDto
                    {
                        Products = products,
                        TotalPages = totalPages
                    };

                    _memoryCache.Set(cacheKey, cacheData, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                        Size = 1024
                    });
                }
                else
                {
                    return View(new ProductsViewModel());
                }

            }

            var viewModel = new ProductsViewModel
            {
                Products = cacheData.Products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = cacheData.TotalPages
            };

            return View(viewModel);
        }

        [HttpPost("Product/UploadPhotoToSearch")]
        [ValidateAntiForgeryToken]
        //ToDO not active
        public async Task<IActionResult> UploadPhotoToSearch(IFormFile file)
        {

            if (file == null)
                return BadRequest(new { Message = "File Empety", success = false });

            var result = await _fileUploadUloudinary.Upload(file);

            if (string.IsNullOrEmpty(result.PublicId))
                return BadRequest(new { Message = "Upload File Is Faild", success = false });

            Console.WriteLine($"result.{result.Url}");

            return Json(new { Message = "Upload File Is Success", success = true, Url = result.Url });

        }

        [Authorize]
        public async Task<IActionResult> AddSallerToFavourit(string userId, string productId, string categoryName, string categoryId)
        {
            try
            {
                Console.WriteLine("Enter in saller methhode");
                var UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (UserId != userId)
                {
                    return Forbid();
                }

                string cacheKeyToProduct = $"Product__{productId}__{CultureInfo.CurrentCulture.Name}";
                var favourit = await unitOfWork.Favorite.GetItemWithFunc(e => e.UserId == userId, new string[] { "FavoriteSallers" });

                if (favourit == null)
                {
                    var user = await unitOfWork.User.GetItemWithFunc(e => e.Id == userId);

                    user.Favorite = new Favorite();
                    unitOfWork.User.UpdateItem(user);
                    await unitOfWork.SaveChangesAsync();
                }


                if (_memoryCache.TryGetValue(cacheKeyToProduct, out ProductDetailsViewModel productDetailsViewModel))
                {
                    Console.WriteLine("Enter in cach");
                    var vendorInfo = productDetailsViewModel.Vendor;


                    var itemSaller = favourit.FavoriteSallers.FirstOrDefault(e => e.VendorId == vendorInfo.Id);
                    if (itemSaller != null)
                    {
                        TempData["ErrorMessage"] = "Saller Aready Exist In Favourits";

                        return RedirectToAction("ProductDetails", new { productId, categoryName, categoryId });
                    }


                    var favouritsallerDto = _mapper.Map<FavoriteSallerDto>(vendorInfo);

                    favourit.FavoriteSallers.Add(_mapper.Map<FavoriteSaller>(favouritsallerDto));
                    unitOfWork.Favorite.UpdateItem(favourit);
                    await unitOfWork.SaveChangesAsync();
                    TempData["SuccesMessage"] = "Added in Saller Favourits Success";
                    return RedirectToAction("ProductDetails", new { productId, categoryName, categoryId });
                }
                Console.WriteLine("Not Found In Cash");
                TempData["ErrorMessage"] = "Added in Saller Favourits Failed";

                return RedirectToAction("ProductDetails", new { productId, categoryName, categoryId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing FavoriteSaller.");
                return View("Error", new ErrorViewModel { RequestId = "401", Message = "An error occurred while processing your request." });
            }
        }

        public class DataReturnToCom
        {
            public bool Status { get; set; }
            public string Message { get; set; }
        }





    }
}

