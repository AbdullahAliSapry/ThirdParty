using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Cart
    {

        public int Id { get; set; }

        public string ProductId { get; set; } = null!;

        public DateTime AddedAt { get; set; }

        // impemation to all propeites

    }
}
