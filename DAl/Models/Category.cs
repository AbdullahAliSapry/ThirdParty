using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class Category
    {
        public string Id { get; set; }

        public string ProviderType { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }
        public bool IsHidden { get; set; }

        public bool IsVirtual { get; set; }

        public string ExternalId { get; set; }

        public bool IsInternal { get; set; }
        public bool IsParent { get; set; }

        //relations
        [JsonIgnore]
        public Category? Parent { get; set; }
        [JsonIgnore]
        public List<Category> SubCategories { get; set; }=new List<Category>();
        public string? ParentId { get; set; }


    }
}
