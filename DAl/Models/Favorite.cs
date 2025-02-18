using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Favorite
    {

        [Key]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public int SoldQuntity { get; set; }



        //relations
        public ICollection<FavouritUser> FavouritUsers { get; set; } = null!;

        public ICollection<ApplicationUser> Users { get; set; } = null!;



    }
}
