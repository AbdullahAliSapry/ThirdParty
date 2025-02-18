
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contracts.SharedDtos
{
    public class CompanyViewModel : RegisterViewModelBase
    {

        [Required(ErrorMessage = "السجل التجاري مطلوب عند اختيار حساب شركة.")]

        public IFormFile? CompanyRecord { get; set; }


    }
}
