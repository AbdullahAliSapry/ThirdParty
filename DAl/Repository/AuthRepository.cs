using Contracts.SharedDtos;
using DAl.IRepository;
using DAl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DAl.Repository
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<ApplicationUser> _userManger;



        public AuthRepository(UserManager<ApplicationUser> userManger)
        {
            _userManger = userManger;
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

            return [..roles];
        }
    }
}
