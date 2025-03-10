using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAl.Models
{

    public class Marketer
    {


        public int Id { get; set; }
        public double CommissionEarned { get; set; } = 0;
        public double TotalSales { get; set; } = 0;
        // ralations



        // ralation between Marketer and ApplicationUser

        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public string UserId { get; set; }

        // ralation between Marketer and CodesToMarketer
        public ICollection<CodesToMarketer> CodesToMarketers { get; set; }


        // relations between MarketerAccount and markeetr

        public ICollection<MarketerAccount> MarketerAccounts { get; set; }


    }
}
