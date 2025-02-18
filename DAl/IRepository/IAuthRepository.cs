using Contracts.SharedDtos;
using DAl.Models;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace DAl.IRepository
{
    public interface IAuthRepository
    {


        Task<IdentityResult> RegisterMethode(ApplicationUser applicationUser, string password, string Role);
        Task<ApplicationUser> LoginMethode(LoginVm loginVm);


        Task<List<string>> GetRoles(ApplicationUser user);



    }
}
