using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

        public async Task Register(RegisterInputModel registerInputModel)
        {
            var user = new User
            {
                UserName = registerInputModel.Username,
                Email = registerInputModel.EmailAddress,
                PhoneNumber = registerInputModel.PhoneNumber
            };

            var identityResult = await this._signInManager.UserManager.CreateAsync(user, registerInputModel.Password);

            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(identityResult.Errors.FirstOrDefault()?.Description);
            }
        }

        public async Task<bool> Login(LoginInputModel loginInputModel) =>
            (await this._signInManager.PasswordSignInAsync(loginInputModel.Username, loginInputModel.Password, true,
                false)).Succeeded;
    }
}