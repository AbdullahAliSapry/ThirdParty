using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string NameofAccount { get; set; } = null!;

        public string NameOfBank {  get; set; } = null!;

        public string NumberOfAccount { get; set; } = null!;

        public bool IsActive { get; set; }

        // relation with payment

        public ICollection<PayMentManoul>? PayMentManouls { get; set; }=new List<PayMentManoul>();

    }
}
