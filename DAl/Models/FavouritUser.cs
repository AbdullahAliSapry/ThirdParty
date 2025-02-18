using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public  class FavouritUser
    {


        public string UserId { get; set; } = null!;

        public string FavoriteId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public Favorite Favorite { get; set; } = null!;

        public DateTime AddedOn { get; set; }= DateTime.UtcNow.ToLocalTime();



    }
}
