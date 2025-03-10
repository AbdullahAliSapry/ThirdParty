using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace DAl.Repository
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<ApplicationUser> _userManger;


        private readonly SignInManager<ApplicationUser> _signInManger;

        private IUnitOfWork<ApplicationUser> _unitOfWork { get; }


        public AuthRepository(UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManger, IUnitOfWork<ApplicationUser> unitOfWork)
        {
            _userManger = userManger;
            _signInManger = signInManger;
            _unitOfWork = unitOfWork;
        }


        public async Task<IdentityResult> RegisterMethode(ApplicationUser User, string password, string Role)
        {


            var result = await _userManger.CreateAsync(User, password);

            if (result.Succeeded)
            {
                await _userManger.AddToRoleAsync(User, Role);
            }

            return result;
        }

        public async Task<ApplicationUser> LoginMethode(LoginVm loginVm)
        {

            var isEmail = new EmailAddressAttribute().IsValid(loginVm.Email);


            var user = isEmail ? await _userManger.FindByEmailAsync(loginVm.Email) :
                await _userManger.Users.FirstOrDefaultAsync(e => e.UserName == loginVm.Email);


            if (user == null)
                return null;

            if (!await _userManger.CheckPasswordAsync(user, loginVm.Password))
            {
                return null;
            }


            return user;
        }

        public async Task<List<string>> GetRoles(ApplicationUser user)
        {


            var roles = await _userManger.GetRolesAsync(user);

            return [.. roles];
        }

        public async Task<bool> UpdateUserNameClaimAsync(ApplicationUser user, string NewValue, string CliamType)
        {

            var claims = await _userManger.GetClaimsAsync(user);

            var nameClaim = claims.FirstOrDefault(c => c.Type == CliamType);

            if (nameClaim != null)
                await _userManger.RemoveClaimAsync(user, nameClaim);

            var addResult = await _userManger.AddClaimAsync(user, new Claim(ClaimTypes.Name, NewValue));


            if (addResult.Succeeded)
            {
                await _signInManger.RefreshSignInAsync(user);
                return true;
            }

            return false;

        }

        public async Task<bool> AddLogicRoleToUserAsync(ApplicationUser user, string role)
        {


            if (role == "Markter")
            {
                var marketer = new Marketer
                {
                    TotalSales = 0,
                    CommissionEarned = 0,
                };
                user.Marketer = marketer;
                var updated = _unitOfWork.User.UpdateItem(user);
                if (!updated) return false;
                var result =  _unitOfWork.SaveChanges();
                return result;

  

            }
            return true;

        }
    }
}
