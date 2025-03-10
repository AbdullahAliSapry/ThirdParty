
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contracts.SharedDtos
{
    public class CompanyViewModel : RegisterViewModelBase
    {

        [Required(ErrorMessage = "نوع الحساب الخاص بك مطلوب")]
        public bool IsComanyOrShop { get; set; }
        public IFormFile? CompanyRecord { get; set; }
        [Required(ErrorMessage ="مكان الشركه مطلوب من فضلك ادخل المكان")]
        public  string Location { get; set; }

    }
}
