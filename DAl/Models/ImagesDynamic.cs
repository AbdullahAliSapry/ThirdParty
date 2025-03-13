using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class ImagesDynamic
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }


        public string PublicId { get; set; }


        public bool IsActive { get; set; }


        public TypeImageUpload typeImageUpload { get; set; }


    }
    public enum TypeImageUpload
    {
        IsLogo,
        Isadvertisement

    }
}
