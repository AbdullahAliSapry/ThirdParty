using AutoMapper;
using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using Infrastructure.ApiotService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdParty.Controllers;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly IUnitOfWork<Category> _unitOfWork;
    private readonly IApiService<CategoryDto> _apiServiceToCategory;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryMenuViewComponent> _logger;
    private IMemoryCache _memoryCache;

    public CategoryMenuViewComponent(
        IUnitOfWork<Category> unitOfWork,
        IApiService<CategoryDto> apiService,
        IMapper mapper,
        ILogger<CategoryMenuViewComponent> logger,
        IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _apiServiceToCategory = apiService;
        _mapper = mapper;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        _logger.LogInformation("🔍 Fetching categories for menu...");
        var categoryParams = new Dictionary<string, string>();
        var key = "Category_Par";

        if (!_memoryCache.TryGetValue(key,out List<Category> categories))
        {
            var allCategories = _unitOfWork.Category.GetItems().ToList();
            if (!allCategories.Any())
            {
                _logger.LogWarning("⚠️ No categories found locally. Fetching from external API...");
                var result = await _apiServiceToCategory.GetDataAsync("GetThreeLevelRootCategoryInfoList", categoryParams);

                if (result?.CategoryInfoList?.Content == null)
                {
                    _logger.LogError("❌ API returned empty or null data.");
                    return View(new List<Category>());
                }

                var categoriesDto = result.CategoryInfoList.Content;
                var sortedCategories = categoriesDto.OrderBy(c => c.Id).ToList();
                var mappedCategories = _mapper.Map<List<Category>>(sortedCategories);

                await _unitOfWork.Category.CreateRange(mappedCategories);
                await _unitOfWork.SaveChangesAsync();

                allCategories = mappedCategories;
                _logger.LogInformation($"✅ Categories fetched and saved: {allCategories.Count} items.");
            }

            var rootCategories = allCategories
                .Where(e => e.ParentId == null)
                .Select(e =>
                {
                    e.SubCategories = e.SubCategories.Take(10)
                        .Select(b =>
                        {
                            b.SubCategories = b.SubCategories.Take(4).ToList();
                            return b;
                        }).ToList();

                    return e;
                })
                .Take(14)
            .ToList();


            _memoryCache.Set(key, rootCategories, new MemoryCacheEntryOptions {

                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 1024

            });
            categories=rootCategories;
            Console.WriteLine("Category Fetaching From database");
        }
        else
        {
            Console.WriteLine("Category Fetaching From chach");

        }



        _logger.LogInformation($"✅ {categories.Count} root categories ready for rendering.");


        return View(_mapper.Map<List<CategoryDto>>(categories));
    }
}
