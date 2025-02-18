using System.ComponentModel.DataAnnotations;

namespace Contracts.SharedDtos
{
    public class PersonalViewModel:RegisterViewModelBase
    {

        [Required]
        public bool IsMarketer { get; set; }

    }

}
