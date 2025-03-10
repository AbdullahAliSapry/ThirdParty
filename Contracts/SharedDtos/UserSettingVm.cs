using System.ComponentModel.DataAnnotations;

namespace Contracts.SharedDtos
{
    public class UserSettingVm
    {

        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage ="الاسم مطلوب")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="برجاء ادخال ايميل صحصح")]
        public string Email { get; set; }
        [Required(ErrorMessage ="رقم الهاتف مطلوب")]
        public string PhoneNumber { get; set; }


    }
}
