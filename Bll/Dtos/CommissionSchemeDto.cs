using DAl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos
{
   public class CommissionSchemeDto
    {

        [Required]
        public string UserType { get; set; }
        [Required]
        public double LowerLimit { get; set; }
        [Required]
        public double UpperLimit { get; set; }
        [Required]
        public double CommissionRate { get; set; }

        public DateTime StartDate { get; set; }= DateTime.UtcNow;

        public DateTime EndDate { get; set; }= DateTime.UtcNow;

        public bool IsActive { get; set; }=true;
    }
}
