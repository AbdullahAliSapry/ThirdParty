using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class PayMentManoul
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string BankName { get; set; } = null!;

        public double Amount { get; set; }
        public string NameAccountTransferFrom { get; set; } = null!;
        public string NumberOfAccountTransferFrom { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsConfirmed { get; set; } = false;

        public DateTime UpatedAt { get; set; } = DateTime.UtcNow;

        // relation with order 
        public Order Order { get; set; } = null!;

        public Guid OrderId { get; set; }

        // relation with user

        public ApplicationUser User { get; set; } = null!;

        public string UserId { get; set; }


        // relation with uploadfiles

        public FileUploads FileUploads { get; set; } = null!;

        public Guid FileId { get; set; }

        // relation between account and payment
        public Account Account { get; set; } = null!;

        public Guid AccountId { get; set; }
    }
}
