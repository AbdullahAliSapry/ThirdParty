using AutoMapper;
using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using Infrastructure.ApiotService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThirdParty.Views.Shared.Components.HeaderActions
{
    public class HeaderActionsViewComponent : ViewComponent
    {
        private readonly IUnitOfWork<Favorite> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryMenuViewComponent> _logger;

        public HeaderActionsViewComponent(IUnitOfWork<Favorite> unitOfWork, IMapper mapper, ILogger<CategoryMenuViewComponent> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public class CartActionsModel
        {
            public int numOfFavourits { get; set; } = 0;
            public int numOfcartitems { get; set; } = 0;
        }
        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {


            var favourit = await _unitOfWork.Favorite.GetItemWithFunc(e => e.UserId == userId, new string[] { "FavoriteItems", "FavoriteSallers" });

            var cart = await _unitOfWork.Cart.GetItemWithFunc(e => e.UserId == userId, new string[] { "CartItems"});

            var model = new CartActionsModel();


            if (favourit == null)
            {
                model.numOfFavourits = 0;
            }

            if (cart == null)
            {
                model.numOfcartitems = 0;

            }

            if (cart != null && cart.CartItems != null)
            {

                cart.CartItems = cart.CartItems.Where(e => !e.IsOredered).ToList();
                model.numOfcartitems = cart.CartItems.Count();

            }   
            
            
            if (favourit != null && favourit.FavoriteItems != null)
            {
                cart.CartItems = cart.CartItems.Where(e => !e.IsOredered).ToList();
                model.numOfFavourits = favourit.FavoriteItems.Count() +favourit.FavoriteSallers.Count();


            }

            return View(model);
        }
    }
}
