using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class BannedIP
    {

        [Key]
        public int Id { get; set; }
        public string IPAddress { get; set; } = null!;
        public int FailureCount { get; set; }
        public DateTime LastFailed { get; set; }
        public DateTime BanUntil { get; set; }
    }
}
