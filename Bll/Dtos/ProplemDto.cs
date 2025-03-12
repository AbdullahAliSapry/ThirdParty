using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bll.Dtos
{
    public class ProplemDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string TypeProplem { get; set; } = null!;
        [Required]
        public string PhoneUser { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        public string DescriptionProplem { get; set; } = null!;
        [Required, EmailAddress]
        public string EmailToUser { get; set; } = null!;
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
        [JsonIgnore]
        public bool IsSolved { get; set; }
    }
}
