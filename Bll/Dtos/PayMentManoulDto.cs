using DAl.Models;
using Microsoft.AspNetCore.Http;
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
    public class PayMentManoulDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "اسم البنك مطلوب")]
        public string BankName { get; set; } = null!;

        [Required(ErrorMessage = "المبلغ مطلوب")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "اسم الحساب المحول منه مطلوب")]
        public string NameAccountTransferFrom { get; set; } = null!;

        [Required(ErrorMessage = "رقم الحساب المحول منه مطلوب")]
        public string NumberOfAccountTransferFrom { get; set; } = null!;

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public bool IsConfirmed { get; set; } = false;

        [JsonIgnore]
        public DateTime UpatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "رقم الطلب مطلوب")]
        public Guid OrderId { get; set; }


        [Required(ErrorMessage = "رقم المستخدم مطلوب")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "رقم الحساب مطلوب")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "صوره التحويل مطلوبه")]
        public IFormFile ReceiptImage { get; set; }=null!;
    }


}
