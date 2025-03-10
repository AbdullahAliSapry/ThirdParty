using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Favorite
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        public DateTime CreatedAt { get; set; }




        // relations


        // realation between user and favourit
        public ApplicationUser User { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public ICollection<FavoriteItem>? FavoriteItems { get; set; } = new List<FavoriteItem>();


        public ICollection<FavoriteSaller> FavoriteSallers { get; set; }=new List<FavoriteSaller>();    



    }
}
