using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class FavoriteSallerDto
    {


        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ProviderType { get; set; } = null!;
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        public string ShopName { get; set; } = null!;
        [Required]
        public string DisplayPictureUrl { get; set; } = null!;
        [Required]
        public int DeliveryScore { get; set; }
        [Required]
        public int ItemScore { get; set; }
        [Required]
        public int ServiceScore { get; set; }
        [Required]
        public double Stars { get; set; }
        [Required]
        public int NumberOfYear { get; set; }

        [Required]
        public string VendorId { get; set; }
    }
}


