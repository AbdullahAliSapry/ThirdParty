using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; } = null!;
        public bool IsComapny { get; set; } = false;
        public bool IsMarketer { get; set; } = false;

        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;

        public DateTime UpdateAt{ get; set; } = DateTime.UtcNow;
        // relations


        public Favorite Favorite { get; set; }


        // ralation between ApplicationUser and Marketer
        public Marketer Marketer { get; set; }

        // relation between user and code 
        public ICollection<ReferralCodeUsage> ReferralCodeUsages { get; set; }

        public  Cart Cart { get; set; } = null!;



        // between  user and company
        public ComapnyAccount? ComapnyAccount { get; set; }

        // relation payment

        public  ICollection<PayMentManoul>? PayMentManoul { get; set; }=new List<PayMentManoul>();

    }

}
