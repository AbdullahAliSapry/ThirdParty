using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class MarketerCodeVm
    {
        [Required(ErrorMessage = "Status tow code is Required")]
        public bool IsActive { get; set; } = true;
        [Required]
        public DateTime RevokedAt { get; set; }
        public double? DiscountRate { get; set; }

        [JsonIgnore]
        public string? Code { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public int? Id { get; set; }

        public ICollection<ReferralCodeUsageDto> ReferralCodeUsages { get; set; }=new List<ReferralCodeUsageDto>();

    }
}
