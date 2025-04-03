using System.ComponentModel.DataAnnotations;

namespace ThirdParty.Utilities
{
    public class ModelUpdatePasseordDto
    {


        [Required]
        public string Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?\""{}|<>]).{8,}$",
ErrorMessage = "الرقم السري يجب أن يحتوي على حرف كبير واحد على الأقل، وحرف صغير واحد على الأقل، ورقم واحد على الأقل، ورمز خاص واحد على الأقل.")]

        public string NewPassword { get; set; }
    }
}
