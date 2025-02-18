using System.ComponentModel.DataAnnotations;

namespace Contracts.SharedDtos
{
    public class RegisterViewModelBase
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "الايميل مطلوب")]
        [EmailAddress(ErrorMessage = "برجاء ادخاال ايميل صحيح")]
        public string Email { get; set; } = null!;
        public string UserName => Email.Split('@')[0];

        [Required(ErrorMessage = "كلمه السر مطلوبه")]

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?\""{}|<>]).{8,}$",
    ErrorMessage = "الرقم السري يجب أن يحتوي على حرف كبير واحد على الأقل، وحرف صغير واحد على الأقل، ورقم واحد على الأقل، ورمز خاص واحد على الأقل.")]

        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "تأكيد كلمه السر مطلوبه")]
        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage = "كلمة السر غير متطابقه")]
        public string ConfirmPassword { get; set; } = null!;
        [Required(ErrorMessage = "كود الدوله مطلوب")]
        public string PhoneNumberCode { get; set; } = null!;
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "كود التأكيد مطلوب")]
        public string VerificationCode { get; set; } = null!;
        [Required]
        public bool IsCompany { get; set; }

 

    }
}
