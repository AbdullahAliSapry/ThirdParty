using DAl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ThirdParty.Utilities
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }




        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {

            var identity = await base.GenerateClaimsAsync(user);


            var nameClaim = identity.FindFirst(ClaimTypes.Name);
            if (nameClaim != null)
                identity.RemoveClaim(nameClaim);

            identity.AddClaim(new Claim(ClaimTypes.Name, user.FullName ?? "Full Name is Null"));

            return identity;


        }


    }
}
