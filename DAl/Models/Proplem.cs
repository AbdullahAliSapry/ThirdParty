using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Proplem
    {


        public int Id { get; set; }
        public string TypeProplem { get; set; } = null!;
        public string PhoneUser { get; set; } = null!;

        public string DescriptionProplem { get; set; } = null!;

        public string EmailToUser { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsSolved { get; set; }

    }
}
