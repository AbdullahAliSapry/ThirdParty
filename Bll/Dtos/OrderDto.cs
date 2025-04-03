using Bll.Dtos.ApiDto;
using Contracts.SharedDtos;
using DAl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class OrderDto
    {

        public Guid Id { get; set; }
        public bool StatusOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdaetdAt { get; set; }
        public double TotalPrice { get; set; }
        public double ShippingPrice { get; set; }
        public double Tax { get; set; }
        public double? TotalTaxWithOutMarkerDiscount { get; set; } = 0.0;
        public bool IsPrivewed { get; set; }
        public bool IsAccesepted { get; set; }
        public bool IsPaid { get; set; } = false;
        public string? CommentOnOrder { get; set; }
        public string? Link1688 { get; set; }
        //public string PhysicalParameters { get; set; }

        public ReferralCodeUsageDto? ReferralCodeUsage { get; set; }


        // relation between order and cartutem

        public ICollection<CartItemDto> CartItems { get; set; }

        //rlelation between Order and payment
        public PayMentManoulDto? PayMentManoul { get; set; }
    }
}
