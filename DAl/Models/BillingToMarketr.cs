using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class BillingToMarketr
    {

        [Key]
        public int Id { get; set; }


        public double Amount { get; set; }

        public DateTime CreateAt { get; set; }

        public string NameBank { get; set; } = null!;
        // relation

        public MarketerAccount MarketerAccount { get; set; }=null!;

        public Guid MarkterAccountId { get; set; }

        // relation between

        public FileUploads FileUploads { get; set; }

        public Guid FileId { get; set; }




    }
}
