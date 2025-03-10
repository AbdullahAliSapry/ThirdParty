using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool StatusOrder { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public DateTime UpdaetdAt { get; set; }
        public double TotalPrice { get; set; }
        public double ShippingPrice { get; set; }
        public double Tax { get; set; }
        public double? TotalTaxWithOutMarkerDiscount { get; set; } = 0.0;
        public bool IsPrivewed { get; set; }
        public bool IsAccesepted { get; set; }
        public bool IsPaid { get; set; }=false;
        public string CommentOnOrder { get; set; }
        //public string PhysicalParameters { get; set; }

        public ReferralCodeUsage? ReferralCodeUsage { get; set; }


        // relation between order and cartutem

        public ICollection<CartItem> CartItems { get; set; }

        //rlelation between Order and payment
        public PayMentManoul? PayMentManoul { get; set; }
    }
}
