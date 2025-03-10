using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class MarketerAccount
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string NameOfBank { get; set; } = null!;

        public TypeOfmethode TypeOfmethode { get; set; }
        public string NameOfAccount { get; set; } = null!;

        public string NumberOfAccount { get; set; } = null!;
        
        public bool IsActived { get; set; }=true;


        // relation between Marketer MarketerAccount
        public Marketer Marketer { get; set; } = null!;

        public int MarketerId { get; set; }

        // relation billing 
        public List<BillingToMarketr>? BillingToMarketrs { get; set; }=new List<BillingToMarketr>(){ };
    }

    public enum TypeOfmethode
    {
        Bank,
        Transfer
    }


}
