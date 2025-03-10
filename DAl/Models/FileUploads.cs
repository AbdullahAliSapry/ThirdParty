using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class FileUploads
    {



        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public DateTime UploadedDate { get; set; }
        public string FilePath { get; set; } = null!;
        // relations

        public ComapnyAccount? ComapnyAccount { get; set; }
        public PayMentManoul? PayMentManoul { get; set; }


        public BillingToMarketr? BillingToMarketr { get; set; }
    }
}
