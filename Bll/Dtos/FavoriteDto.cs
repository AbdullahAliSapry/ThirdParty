using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class FavoriteDto
    {
        
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public int SoldQuntity { get; set; }

    }
}
