using Bll.Dtos.ApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class CartDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }


        //relaton
        public string UserId { get; set; } = null!;

        public ICollection<CartItemDto> CartItems { get; set; } = null!;
    }
}
