using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class PricesToshipping
    {


        public int Id { get; set; }
        public TypeToShiping TypeToShiping { get; set; }
        public double LowerLimit { get; set; }
        public double UpperLimit { get; set; }
        public double Price { get; set; }
        public  DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsActived { get; set; }



    }


    public enum TypeToShiping
    {

        Weight,
        Volume


    }

}
