using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class UpdateQuntityInfoDto
    {

        [Required]
        public int newquntity { get; set; }
        [Required]
        public bool isquntityAttrbute { get; set; }
        [Required]
        public string CartId { get; set; }
        [Required]
        public int CartitemId { get; set; }
        public int? ItemAttrbuteId { get; set; }
    }
}
