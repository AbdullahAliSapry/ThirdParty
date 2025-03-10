using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class FavoriteSaller
    {


        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ProviderType { get; set; }=null!;
        public string DisplayName { get; set; } = null!;

        public string ShopName { get; set; } = null!;
        
        public string DisplayPictureUrl { get; set; } = null!;  

        public double DeliveryScore { get; set; }

        public double ItemScore { get; set; }


        public string VendorId { get; set; } = null!;

        public double ServiceScore { get; set; }

        public double Stars {  get; set; }

        public double NumberOfYear { get; set; }

        // relations

        public Favorite Favorite { get; set; } = null!;

        public Guid FavoriteId { get; set; }
    }
}
