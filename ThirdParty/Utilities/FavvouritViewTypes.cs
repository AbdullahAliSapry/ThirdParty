using Bll.Dtos;

namespace ThirdParty.Utilities
{
    public class FavvouritViewTypes
    {

        public List<FavoriteItemDto> FavoriteItems { get; set; } = new List<FavoriteItemDto>();

        public List<FavoriteItemDto> FAvouritSaller { get; set; } = new List<FavoriteItemDto>();
    }
}
