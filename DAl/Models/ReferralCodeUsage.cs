using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class ReferralCodeUsage
    {


        public string UserId { get; set; }

        public int CodeId { get; set; }

        public DateTime CreateAt { get; set; }= DateTime.Now;
        // relations
        public CodesToMarketer CodesToMarketer { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public Order Order { get; set; } = null!;

        public Guid OrderId { get; set; }


        



    }
}
