using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class LoginVm
    {

        [Required(ErrorMessage = "البيريد الاكتروني او اسم السمتدم")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمه السر مطلوبه")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



    }
}
