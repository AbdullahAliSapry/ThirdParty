using Contracts.SharedDtos;
using System.ComponentModel.DataAnnotations;

namespace ThirdParty.CustomValdtion
{
    public class RecordValidationAttribute:ValidationAttribute
    {
        private readonly string _Message;
        public RecordValidationAttribute(string message= "السجل التجاري مطلوب عند اختيار حساب شركة.")
        {
            _Message = message;
        }




        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var Userview = (RegisterViewModelBase)validationContext.ObjectInstance;

            if (Userview.IsCompany)
            {

                var file = value as IFormFile;

                if (file==null || file.Length==0)
                {
                    return new ValidationResult(_Message);
                }

            }


            return ValidationResult.Success;

        }




    }

   
}
