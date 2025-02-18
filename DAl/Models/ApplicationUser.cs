using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAl.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; } = null!;
        public bool IsComapny { get; set; } = false;
        public bool IsMarketer { get; set; } = false;

        // relations

        public ICollection<FavouritUser> FavouritUsers { get; set; } = null!;

        public ICollection<Favorite> Favorites { get; set; } = null!;

    }

}
