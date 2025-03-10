using Contracts.Helpers;
using Contracts.SharedDtos;
using DAl.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos.ApiDto
{


    public class CartItemDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }

        public string ProductId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Comment { get; set; }
        public string PhysicalParameters { get; set; } = null!;
        public double PricePerPiece { get; set; }
        public double Discount { get; set; }
        public bool IsOredered { get; set; }
        public int Quntity { get; set; }
        public double FinalPrice => PricePerPiece * Quntity;

        public ICollection<AttributeItemDto> AttributeItems { get; set; } = new List<AttributeItemDto>();

        [System.Text.Json.Serialization.JsonIgnore]
        public CategoryDto? Category { get; set; }
        [Required]
        public string categoryId { get; set; }

        [NotMapped]
        [JsonIgnore]

        public PhysicalParameters PhysicalParametersJson
        {
            get => JsonConvert.DeserializeObject<PhysicalParameters>(PhysicalParameters) ?? new PhysicalParameters();
            set => PhysicalParameters = JsonConvert.SerializeObject(value);
        }


    }


}
