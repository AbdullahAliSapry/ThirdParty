using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{

    public class Marketer
    {


        public int Id { get; set; }
        public int AllUsesToCode { get; set; }
        public string? BankAccount { get; set; }
        public decimal CommissionEarned { get; set; } = 0;
        public decimal TotalSales { get; set; } = 0; 
        // ralations
        public ICollection<CodesToMarketer> CodesToMarketers { get; set; }



    }
}
