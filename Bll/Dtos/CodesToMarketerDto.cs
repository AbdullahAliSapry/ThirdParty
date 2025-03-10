using DAl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class CodesToMarketerDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RevokedAt { get; set; }
        public double? DiscountRate { get; set; }
        public Marketer Marketer { get; set; } = null!;
        public int MarketerId { get; set; }
        public ICollection<ReferralCodeUsage> ReferralCodeUsages { get; set; }=new List<ReferralCodeUsage>();
    }
}
