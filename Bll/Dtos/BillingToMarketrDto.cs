using DAl.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class BillingToMarketrDto
    {

        [Required]
        public double Amount { get; set; }
        [Required]

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string NameBank { get; set; } = null!;
        [Required]
        public string MarkterAccountId { get; set; } = null!;
        [Required]
        public IFormFile Image { get; set; }=null!;




    }
}
