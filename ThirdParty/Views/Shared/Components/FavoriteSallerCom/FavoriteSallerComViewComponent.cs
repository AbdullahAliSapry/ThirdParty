using AutoMapper;
using Bll.Dtos;
using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using ThirdParty.ViewModels;
using Twilio.Rest.Trunking.V1;

namespace ThirdParty.Views.Shared.Components.FavoriteSallerCom
{
    public class FavoriteSallerComViewComponent : ViewComponent
    {
        private readonly IUnitOfWork<FavoriteSaller> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoriteSallerComViewComponent> _logger;
        private readonly IMemoryCache _memoryCache;

        public FavoriteSallerComViewComponent(IUnitOfWork<FavoriteSaller> unitOfWork, IMapper mapper, ILogger<FavoriteSallerComViewComponent> logger, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {



            var allSallerFavourits =await _unitOfWork.Favorite.GetItemWithFunc(e=>e.UserId==userId,new string[] { "FavoriteSallers" });


            Console.WriteLine(allSallerFavourits.FavoriteSallers.Count);

            return View(_mapper.Map<List<FavoriteSallerDto>>(allSallerFavourits.FavoriteSallers));


        }

  
    }
}
