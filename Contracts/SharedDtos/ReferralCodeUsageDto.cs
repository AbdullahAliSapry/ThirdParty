using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{
    public class ReferralCodeUsageDto
    {
        public string UserId { get; set; }

        public int CodeId { get; set; }

        public DateTime CreateAt { get; set; }
        public Guid OrderId { get; set; }

    }
}
