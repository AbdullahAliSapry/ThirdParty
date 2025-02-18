using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SharedDtos
{

    public class CategoryDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? NameAr { get; set; }
        public string ProviderType { get; set; } = null!;
        public string ExternalId { get; set; } = null!;
        public bool IsVirtual { get; set; }
        public bool IsParent { get; set; }
        public bool IsInternal { get; set; }
        public bool IsHidden { get; set; }
        public string? ParentId { get; set; }

        public List<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();

    }
}
