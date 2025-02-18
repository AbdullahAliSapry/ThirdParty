using Bll.Dtos;



namespace ThirdParty.ViewModels
{
    public class ProductsViewModel
    {
        public List<ProductDto> Products { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
