using Contracts.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class AttributeItem
    {
        [Key]
        public int Id { get; set; }

        // تخزين الـ Attributes ديناميكيًا كـ JSON
        public string AttributesJson { get; set; } = null!;

        public int Quantity { get; set; }

        public int CartItemId { get; set; }
        public CartItem CartItem { get; set; } = null!;

        [NotMapped]
        public Dictionary<string, AttributeDetail> Attributes
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, AttributeDetail>>(AttributesJson) ?? new();
            set => AttributesJson = JsonConvert.SerializeObject(value);
        }
    }

}
