using Bll.Dtos;

namespace ThirdParty.ViewModels
{
    public class FavoutitsVm
    {

        public List<FavoriteDto> FavoritesProduct { get; set; }=new List<FavoriteDto>();

        public List<FavoriteDto> FavoritesService { get; set; } = new List<FavoriteDto>();  
    }
}
