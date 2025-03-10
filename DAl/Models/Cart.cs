using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Cart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }


        //relaton
        public  ApplicationUser User{ get; set; } = null!;
        public string UserId { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();


    }
}
