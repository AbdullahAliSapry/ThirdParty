using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class MarketerVm
    {


        public int Id { get; set; }
        public double CommissionEarned { get; set; } = 0;
        public double TotalSales { get; set; } = 0;
       
        public ICollection<MarketerCodeVm> CodesToMarketers { get; set; }

    }
}
