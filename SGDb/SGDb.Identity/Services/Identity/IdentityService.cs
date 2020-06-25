using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure;
using SGDb.Identity.Data.Models;
using SGDb.Identity.Models.Identity;
using SGDb.Identity.Services.Identity.Contracts;

namespace SGDb.Identity.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<User> _signInManager;

        public IdentityService(SignInManager<User> signInManager)
        {
            this._signInManager = signInManager;
        }

        public async Task<string> GetUserRole(string email)
        {
            var user = await this._signInManager.UserManager.FindByEmailAsync(email);
            
            if(user == null)
                throw new InvalidOperationException("User not found.");
            
            return (await this._signInManager.UserManager.GetRolesAsync(user)).FirstOrDefault();
        }

        public async Task Register(RegisterInputModel registerInputModel)
        {
            var user = new User
            {
                FirstName = registerInputModel.FirstName,
                LastName = registerInputModel.LastName,
                UserName = registerInputModel.EmailAddress,
                Email = registerInputModel.EmailAddress,
                PhoneNumber = registerInputModel.PhoneNumber
            };

            var identityResult = await this._signInManager.UserManager.CreateAsync(user, registerInputModel.Password);

            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(identityResult.Errors.FirstOrDefault()?.Description);
            }
            
            var usersCount = await this._signInManager.UserManager.Users.CountAsync();
            
            if (usersCount < 2)
            {
                await this._signInManager.UserManager.AddToRoleAsync(user, RolesConstants.Administrator);
            }
            else if (usersCount < 3)
            {
                await this._signInManager.UserManager.AddToRoleAsync(user, RolesConstants.Creator);
            }
            else
            {
                await this._signInManager.UserManager.AddToRoleAsync(user, RolesConstants.User);
            }
        }

        public async Task<bool> Login(LoginInputModel loginInputModel) =>
            (await this._signInManager.PasswordSignInAsync(loginInputModel.EmailAddress, loginInputModel.Password, true,
                false)).Succeeded;
    }
}