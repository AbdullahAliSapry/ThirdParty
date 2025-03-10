using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bll.Dtos
{
   public class FavoriteItemDto
    {


        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Image { get; set; }
        public double Price { get; set; }
        public string ProductId { get; set; } = null!;
        public string? MoreNotes { get; set; }
        public string? VendorName { get; set; }
        public string? VendorId { get; set; }
        public int Quntity { get; set; }
        public string? VendorLink { get; set; }

        public double TotalPrice => Quntity * Price;



        // relation
        public string CategoryId { get; set; } = null!;

    }
}
