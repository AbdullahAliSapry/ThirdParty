using AutoMapper;
using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Mvc;

namespace ThirdParty.Views.Shared.Components.LogoCom
{
    public class LogoComViewComponent : ViewComponent
    {
        private readonly IUnitOfWork<ImagesDynamic> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryMenuViewComponent> _logger;

        public LogoComViewComponent(IUnitOfWork<ImagesDynamic> unitOfWork, IMapper mapper, ILogger<CategoryMenuViewComponent> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {


            var logo = _unitOfWork.ImagesDynamic
                .GetItemsWithFunc(e => e.IsActive && e.typeImageUpload == TypeImageUpload.IsLogo)
                .FirstOrDefault();
            if (logo == null)
            {
                Console.WriteLine("Enter in null");
            }

            return View(logo);
        }

    }
}
