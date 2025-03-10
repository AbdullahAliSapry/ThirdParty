using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class ComapnyAccount
    {



        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Location { get; set; } = null!;

        public bool IsComanyOrShop { get; set; }

        public FileUploads? FileUploads { get; set; }

        public Guid? FileId { get; set; }


        public ApplicationUser User { get; set; }=null!;
        public string UserId { get; set; } = null!;

    }
}
