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
namespace ThirdParty.Controllers
{
    public class HomeController : Controller
    {


        private IUnitOfWork<Category> _unitOfWork;
        private readonly ILogger<HomeController> _logger;
        public IMapper _mapper { get; }
        private IApiService<CategoryDto> _apiServiceToCategory;

        private readonly ISendEmail _SendEmail;

        private IHtmlLocalizer<HomeController> _localizer;
        private IStringLocalizer<HomeController> _localizerString;

        private ITransaltionService _TransaltionService { get; }

        private List<CategoryDto> _categories = new List<CategoryDto>();
        public HomeController(ILogger<HomeController> logger, IApiService<CategoryDto> apiService,
            ISendEmail sendEmail, IUnitOfWork<Category> unitOfWork, IMapper mapper, ITransaltionService transaltionService, IHtmlLocalizer<HomeController> localizer, IStringLocalizer<HomeController> localizerString)
        {
            _logger = logger;
            _apiServiceToCategory = apiService;
            _SendEmail = sendEmail;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _TransaltionService = transaltionService;
            _localizer = localizer;
            _localizerString = localizerString;
        }


        public async Task<IActionResult> Index()
        {
            try
            {


                var CatgoryParams = new Dictionary<string, string>();

                var allcategory = _unitOfWork.Category.GetItems();

                if (allcategory.Count == 0)
                {

                    var result = await _apiServiceToCategory.GetDataAsync("GetThreeLevelRootCategoryInfoList", CatgoryParams);

                    _categories = result.CategoryInfoList.Content;

                    var sortedCategories = _categories.OrderBy(c => c.Id).ToList();

                    var mappedCategories = _mapper.Map<List<Category>>(sortedCategories);

                    await _unitOfWork.Category.CreateRange(mappedCategories);

                    await _unitOfWork.SaveChangesAsync();

                    allcategory = mappedCategories;

                }
                
                var rootsCategory = allcategory.
                    Where(e => e.ParentId == null).ToList();
                
                ViewBag.Catgory = _mapper.Map<List<CategoryDto>>(rootsCategory);

                return View();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"General Error: {ex.Message}");

                return View();

            }
        }

        //public IActionResult SetLanguage(string culture, string returnUrl)
        //{
        //    Response.Cookies.Append(
        //        CookieRequestCultureProvider.DefaultCookieName,
        //        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        //        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        //    );

        //    return LocalRedirect(returnUrl);
        //}



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


    }
}
