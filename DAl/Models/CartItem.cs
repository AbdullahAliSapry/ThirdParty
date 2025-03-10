using Contracts.Helpers;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class CartItem
    {

        [Key]
        public int Id { get; set; }
        public string ProductId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string Comment { get; set; }
        public double PricePerPiece { get; set; }
        public double Discount { get; set; }
        public int Quntity { get; set; }
        public string PhysicalParameters { get; set; }=null!;
        public bool IsOredered { get; set; } = false;

        //relations
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }

        public ICollection<AttributeItem> AttributeItems { get; set; } = new List<AttributeItem>();

        // relation with category

        public Category? Category { get; set; }

        public string? categoryId { get; set; }



        // relation between cart item and order
        public Order? Order { get; set; }

        public Guid? OrderId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public PhysicalParameters PhysicalParametersJson
        {
            get => JsonConvert.DeserializeObject<PhysicalParameters>(PhysicalParameters) ?? new PhysicalParameters();
            set => PhysicalParameters = JsonConvert.SerializeObject(value);
        }
    }
  

}
