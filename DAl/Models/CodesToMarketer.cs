using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class CodesToMarketer
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime RevokedAt { get; set; }


        // relations


        public Marketer Marketer { get; set; } = null!;


        public int MarketerId { get; set; }

    }
}
