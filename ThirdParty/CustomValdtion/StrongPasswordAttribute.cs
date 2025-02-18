using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ThirdParty.CustomValdtion
{
    public class StrongPasswordAttribute : ValidationAttribute
    {


        private readonly Dictionary<string, string> _patterns = new Dictionary<string, string>{
            { @"[A-Z]", "كلمة السر يجب أن تحتوي على حرف كبير." },
            {@"[a-z]",  "كلمة السر يجب أن تحتوي على حرف صغير."},
            {@"[0-9]", "كلمة السر يجب أن تحتوي على رقم."},
            { @"[!@#$%^&*(),.?\""{}|<>]", "كلمة السر يجب أن تحتوي على رمز خاص."},
           { @"^.{8,}$", "كلمة السر يجب أن تحتوي على 8 أحرف على الأقل."}

        };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Password is required");
            }
            string password = value.ToString();
            foreach (var pattern in _patterns)
            {
                if (!Regex.IsMatch(password, pattern.Key))
                {
                    var errorMessage = pattern.Value;
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;

        }
    }
}
