using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "الايميل الخاص بك")]
        public string Email { get; set; }
        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبه")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?\""{}|<>]).{8,}$",
    ErrorMessage = "الرقم السري يجب أن يحتوي على حرف كبير واحد على الأقل، وحرف صغير واحد على الأقل، ورقم واحد على الأقل، ورمز خاص واحد على الأقل.")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور الجديدة")]
        [Required(ErrorMessage = " تاكيد كلمة المرور الجديدة مطلوبه")]
        [Compare("Password", ErrorMessage = "كلمات المرور غير متطابقة.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }



    }
}

