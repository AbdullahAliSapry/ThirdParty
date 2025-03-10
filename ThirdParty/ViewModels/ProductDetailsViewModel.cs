using Bll.Dtos;

namespace ThirdParty.ViewModels
{
    public class ProductDetailsViewModel
    {


        public ProductDto Product { get; set; } = new ProductDto();

        public VendorDto Vendor { get; set; } = new VendorDto();
        public VendorItemDto VendorItems { get; set; } = new VendorItemDto();

        public ProviderReviews Reviews { get; set; } = new ProviderReviews();


    }
}
