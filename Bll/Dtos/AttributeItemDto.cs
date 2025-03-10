using Contracts.Helpers;
using DAl.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Bll.Dtos
{
    public class AttributeItemDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string AttributesJson { get; set; } = null!;

        public int Quantity { get; set; }

        [NotMapped]
        public Dictionary<string, AttributeDetail> Attributes
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, AttributeDetail>>(AttributesJson) ?? new();
            set => AttributesJson = JsonConvert.SerializeObject(value);
        }

    }
}
