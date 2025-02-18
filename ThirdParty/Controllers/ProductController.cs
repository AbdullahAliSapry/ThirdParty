using ThirdParty.ViewModels;
using Bll.Dtos;
using Contracts.SharedDtos;
using Infrastructure.ApiotService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Globalization;
using System;

namespace ThirdParty.Controllers
{
    public class ProductController : Controller
    {

        private IApiService<object> _apiServiceToProduct;
        private readonly IMemoryCache _memoryCache;

        public ProductController(IApiService<object> apiService, IMemoryCache memoryCache)
        {
            _apiServiceToProduct = apiService;
            _memoryCache = memoryCache;
        }
        public async Task<IActionResult> ProductsToCategory(string categoryName, string categoryId, int page = 1, int pageSize = 20)
        {

            string cacheKey = $"Products_{categoryId}_{page}_{CultureInfo.CurrentCulture.Name.ToString().Trim()}";
            int totalItems = 0;
            int totalPages = 0;

            Console.WriteLine(cacheKey);

            if (!_memoryCache.TryGetValue(cacheKey, out ProductsCacheDto cacheData))
            {
                Console.WriteLine("Fetching data from API...");

                var Params = new Dictionary<string, string>
                            {
                                {"framePosition", ((page - 1) * pageSize).ToString()},
                                {"frameSize", pageSize.ToString()},
                                {"xmlParameters", $"<SearchItemsParameters><CategoryId>{categoryId}</CategoryId></SearchItemsParameters>"},
                                {"blockList", "" },
                                {"signature", "" },
                                {"timestamp", "" },
                                {"language", CultureInfo.CurrentCulture.Name.ToString()}
                            };


                dynamic response = await _apiServiceToProduct.GetDataAsyncDynmic("BatchSearchItemsFrame", Params);
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

                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                        Size = 1024
                    };

                    _memoryCache.Set(cacheKey, cacheData, cacheEntryOptions);
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

            var viewModel = new ProductsViewModel
            {
                Products = cacheData.Products,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = cacheData.TotalPages
            };

            return View(viewModel);
        }


        public async Task<IActionResult> ProductDetails(string productId, string categoryId, string categoryName, int page)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("Product ID is required.");
            }

            string cacheKeyToProducts = $"Products_{categoryId}_{page + 1}_{CultureInfo.CurrentCulture.Name.Trim()}";
            string cacheKeyToProduct = $"Product__{productId}__{CultureInfo.CurrentCulture.Name}";

            Console.WriteLine($"Product ID: {productId}");

            var Params = new Dictionary<string, string>
                        {
                            {"itemId", productId},
                            {"language", CultureInfo.CurrentCulture.Name},
                            {"blockList", ""}
                        };





            if (_memoryCache.TryGetValue(cacheKeyToProduct, out ProductDto productDto))
            {
                Console.WriteLine("✅ Product found in cache.");
                return View(productDto);
            }



            if (!_memoryCache.TryGetValue(cacheKeyToProducts, out ProductsCacheDto productsCacheDto))
            {
                Console.WriteLine("🌐 Fetching product data from API...");

                dynamic response = await _apiServiceToProduct.GetDataAsyncDynmic("BatchGetItemFullInfo", Params);

                var jsonString = JsonConvert.SerializeObject(response);
                var myData = JsonConvert.DeserializeObject<ApiProductDeatilsDto<ProductDto>>(jsonString);

                if (myData?.Result?.Item == null)
                {
                    return NotFound($"❌ Product with ID {productId} not found in API.");
                }

                productDto = myData.Result.Item;

                _memoryCache.Set(cacheKeyToProduct, productDto, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Size = 1024
                });

                return View(productDto);
            }



            var products = _memoryCache.Get<ProductsCacheDto>(cacheKeyToProducts);



            if (products?.Products == null || !products.Products.Any())
            {
                return NotFound("❌ Products not found.");
            }


            var product = products.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound($"❌ Product with ID {productId} not found.");
            }

            return View(product);
        }

        public async Task<IActionResult> SearchOnProduct()
        {





            return View();


        }


        public class ProductsCacheDto
        {
            public List<ProductDto> Products { get; set; }
            public int TotalPages { get; set; }
        }


        //public async Task<IActionResult> ProductsToCategory(string categoryName, string categoryId, int page = 1, int pageSize = 20)
        //{
        //    string cacheKey = $"Products_{categoryId}_{page}_{pageSize}";
        //    int totalItems = 0;
        //    int totalPages = 0;

        //    if (!_memoryCache.TryGetValue(cacheKey, out ProductsCacheDto cacheData))
        //    {
        //        Console.WriteLine("Fetching data from API...");

        //        var Params = new Dictionary<string, string>
        //{
        //    {"framePosition", ((page - 1) * pageSize).ToString()},
        //    {"frameSize", (pageSize / 2).ToString()},
        //    {"xmlParameters", $"<SearchItemsParameters><CategoryId>{categoryId}</CategoryId></SearchItemsParameters>"},
        //    {"blockList", "" },
        //    {"signature", "" },
        //    {"timestamp", "" },
        //    {"language", "en"}
        //};

        //        var responseTask = _apiServiceToProduct.GetDataAsyncDynmic("BatchSearchItemsFrame", Params);
        //        var response2Task = _apiServiceToProduct.GetDataAsyncDynmic("BatchSearchItemsFrame", Params);

        //        await Task.WhenAll(responseTask, response2Task);

        //        dynamic response = await responseTask;
        //        dynamic response2 = await response2Task;

        //        var jsonSerializerSettings = new JsonSerializerSettings
        //        {
        //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //        };

        //        string jsonString = JsonConvert.SerializeObject(response, jsonSerializerSettings);
        //        string jsonString2 = JsonConvert.SerializeObject(response2, jsonSerializerSettings);

        //        var myData = JsonConvert.DeserializeObject<ApiResponseDtoTow<ProductDto>>(jsonString);
        //        var myData2 = JsonConvert.DeserializeObject<ApiResponseDtoTow<ProductDto>>(jsonString2);

        //        if (myData?.Result?.Items?.Items?.Content != null)
        //        {
        //            var products = myData.Result.Items.Items.Content;
        //            totalItems = myData.Result.Items.Items.TotalCount;
        //            totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        //            products.AddRange(myData2?.Result?.Items?.Items?.Content);
        //            cacheData = new ProductsCacheDto
        //            {
        //                Products = products,
        //                TotalPages = totalPages
        //            };

        //            var cacheEntryOptions = new MemoryCacheEntryOptions
        //            {
        //                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
        //                SlidingExpiration = TimeSpan.FromMinutes(2),
        //                Size = 1024
        //            };

        //            _memoryCache.Set(cacheKey, cacheData, cacheEntryOptions);
        //        }
        //        else
        //        {
        //            Console.WriteLine("API returned empty data!");
        //            return View(new ProductsViewModel());
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Data found in cache");
        //    }

        //    var viewModel = new ProductsViewModel
        //    {
        //        Products = cacheData.Products,
        //        CurrentPage = page,
        //        PageSize = pageSize,
        //        TotalPages = cacheData.TotalPages
        //    };

        //    return View(viewModel);
        //}


    }
}
