using DAl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class MarketerAccountDto
    {

        [JsonIgnore]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Bank name is required.")]
        public string NameOfBank { get; set; } = string.Empty;

        public TypeOfmethode TypeOfmethode { get; set; } = TypeOfmethode.Bank;

        [Required(ErrorMessage = "Account holder name is required.")]
        public string NameOfAccount { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account number is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Account number must be numeric.")]
        public string NumberOfAccount { get; set; } = string.Empty;

        [Required]
        public int MarketerId { get; set; }
    }
}
