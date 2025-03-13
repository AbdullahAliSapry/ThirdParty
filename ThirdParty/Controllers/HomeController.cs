using System.Diagnostics;
using ThirdParty.ViewModels;
using AutoMapper;
using DAl.IRepository;
using DAl.Models;
using Infrastructure.ApiotService.Interfaces;
using Infrastructure.SendingEmails;
using Microsoft.AspNetCore.Mvc;
using Contracts.SharedDtos;
using Infrastructure.Translationservice;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Bll.Dtos;
using ThirdParty.Utilities;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json;
namespace ThirdParty.Controllers
{
    public class HomeController : Controller
    {


        private IUnitOfWork<Category> _unitOfWork;
        private readonly ILogger<HomeController> _logger;
        public IMapper _mapper { get; }
        private IApiService<CategoryDto> _apiServiceToCategory;
        private IApiService<object> _apiServiceToProduct;

        private readonly ISendEmail _SendEmail;

        private IHtmlLocalizer<HomeController> _localizer;
        private IStringLocalizer<HomeController> _localizerString;

        private ITransaltionService _TransaltionService { get; }

        private List<CategoryDto> _categories = new List<CategoryDto>();

        private IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, IApiService<CategoryDto> apiService,
            ISendEmail sendEmail, IUnitOfWork<Category> unitOfWork, IMapper mapper, ITransaltionService transaltionService, IHtmlLocalizer<HomeController> localizer, IStringLocalizer<HomeController> localizerString, IMemoryCache cache, IApiService<object> apiServiceToProduct)
        {
            _logger = logger;
            _apiServiceToCategory = apiService;
            _SendEmail = sendEmail;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _TransaltionService = transaltionService;
            _localizer = localizer;
            _localizerString = localizerString;
            _cache = cache;
            _apiServiceToProduct = apiServiceToProduct;
        }


        public async Task<IActionResult> Index()
        {
            try
            {

                var cacheKey = "Popular_product_and_Last_Products";
                var towTypeProducts = new TowTypeProducts
                {
                    ProductsLast = new List<ProductDto>(),
                    ProductsPopular = new List<ProductDto>(),
                    ProductsNew = new List<ProductDto>(),
                };

                if (_cache.TryGetValue(cacheKey, out TowTypeProducts cachedProducts))
                {
                    Console.WriteLine("✅ Products fetched from cache.");
                    ViewBag.Products = cachedProducts;
                }
                else
                {
                    Console.WriteLine("⚠️ Products not found in cache. Fetching from API...");

                    var publicclassToXml = new RatingListItemSearchParameters
                    {
                        CategoryId = 0,
                        ItemRatingType = "Popular"
                    };

                    var xmlparams = XmlHelper.ConvertToXml(publicclassToXml);
                    var productsParams = new Dictionary<string, string>
                            {
                                { "framePosition", "0" },
                                { "frameSize", "20" },
                                { "xmlSearchParameters", xmlparams },
                                { "language", CultureInfo.CurrentCulture.Name }
                            };

                    // popular
                    var popularResult = await _apiServiceToProduct.GetDataAsyncDynmic("SearchRatingListItems", productsParams);
                    var popularResultString = JsonConvert.SerializeObject(popularResult);
                    var products = JsonConvert.DeserializeObject<ApiTypeThree<ProductDto>>(popularResultString);

                    // latest
                    publicclassToXml.ItemRatingType = "Last";
                    productsParams["xmlParameters"] = XmlHelper.ConvertToXml(publicclassToXml);

                    var lastResult = await _apiServiceToProduct.GetDataAsyncDynmic("SearchRatingListItems", productsParams);
                    var lastResultString = JsonConvert.SerializeObject(lastResult);
                    var productsLast = JsonConvert.DeserializeObject<ApiTypeThree<ProductDto>>(lastResultString);



                    var publicclasToXmlnew = new SearchItemsParameters
                    {
                        StuffStatus = "New",
                        ItemTitle = "laptops",
                    };
                    productsParams["xmlParameters"] = XmlHelper.ConvertToXml(publicclasToXmlnew);
                    productsParams["blockList"] = "";
                    productsParams.Remove("xmlSearchParameters");

                    // new
                    var NewProductResult = await _apiServiceToProduct.GetDataAsyncDynmic("BatchSearchItemsFrame", productsParams);
                    var NewProductResultString = JsonConvert.SerializeObject(NewProductResult);
                    var NewProducts = JsonConvert.DeserializeObject<ApiResponseDtoTow<ProductDto>>(NewProductResultString);


                    if (products?.OtapiItemInfoSubList?.Content == null)
                        Console.WriteLine("❌ Popular products API returned null.");

                    if (productsLast?.OtapiItemInfoSubList?.Content == null)
                        Console.WriteLine("❌ Last products API returned null.");

                    if (NewProducts?.Result?.Items.Items.Content == null)
                        Console.WriteLine("❌ new products API returned null.");

                    if (products?.OtapiItemInfoSubList?.Content != null && productsLast?.OtapiItemInfoSubList?.Content != null)
                    {
                        towTypeProducts.ProductsPopular = products.OtapiItemInfoSubList.Content;
                        towTypeProducts.ProductsLast = productsLast.OtapiItemInfoSubList.Content;
                        towTypeProducts.ProductsNew = NewProducts?.Result?.Items.Items.Content;
                        _cache.Set(cacheKey, towTypeProducts, new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                            SlidingExpiration = TimeSpan.FromMinutes(2),
                            Size = 1024
                        });

                        Console.WriteLine("✅ Products fetched from API and cached.");
                        ViewBag.Products = towTypeProducts;
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Could not fetch both Popular and Last products.");
                        ViewBag.Products = new TowTypeProducts
                        {
                            ProductsLast = new List<ProductDto>(),
                            ProductsPopular = new List<ProductDto>(),
                            ProductsNew = new List<ProductDto>(),


                        };
                    }



                }
                var images = _unitOfWork.ImagesDynamic
                    .GetItemsWithFunc(e=>e.IsActive && e.typeImageUpload==TypeImageUpload.Isadvertisement);
                ViewData["Images"]=images;
                ViewData["ImageLogo"] = _unitOfWork.ImagesDynamic
                    .GetItemsWithFunc(e => e.IsActive && e.typeImageUpload == TypeImageUpload.IsLogo).SingleOrDefault() ;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ General Error: {ex.Message}");
                return View();
            }
        }


        void AttachSubCategories(List<Category> categories, List<Category> allCategories)
        {
            foreach (var category in categories)
            {
                var subCategories = allCategories
                    .Where(e => e.ParentId == category.Id)
                    .ToList();

                //foreach (var sub in subCategories)
                //{
                //    if (!category.SubCategories.Any(s => s.Id == sub.Id))
                //    {
                //        category.SubCategories.Add(sub);
                //    }
                //}

                AttachSubCategories(subCategories, allCategories);
            }
        }

        public async Task<IActionResult> GetCategory()
        {
            if (_categories.Count == 0)
            {

                _categories = _mapper.Map<List<CategoryDto>>
                    (_unitOfWork.Category.GetItems(getRel: false).FindAll(e => e.ParentId == null));

            }

            ViewBag.Categories = _categories.ToList();

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {


            return View(new ErrorViewModel
            {
                RequestId
                = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });



        }



        [HttpGet]
        public IActionResult Calculator()
        {
            // 1- get price to shpiipng

            var shippings = _unitOfWork.PricesToshipping.
                GetItemsWithFunc(e => e.TypeToShiping == TypeToShiping.Weight && e.IsActived);

            return View(shippings);
        }


        public IActionResult HelpCenter()
        {

            return View();

        }

        [HttpPost("/Home/HelpCenterData")]
        [ValidateAntiForgeryToken]
        public IActionResult HelpCenterData([FromBody] ProplemDto proplemDto)
        {

            if (!ModelState.IsValid)
                return Json(new { Message = "Valdation Error", ModelState });

            var Proplem = _mapper.Map<Proplem>(proplemDto);

            _unitOfWork.Proplem.Create(Proplem);

            _unitOfWork.SaveChanges();

            return Json(new { Message = "The data has been " +
                "sent successfully. You will be contacted as " +
                "soon as possible.",  });

        }

    }


    public class TowTypeProducts
    {

        public List<ProductDto>? ProductsPopular { get; set; }
        public List<ProductDto>? ProductsLast { get; set; }
        public List<ProductDto>? ProductsNew { get; set; }


    }
}
