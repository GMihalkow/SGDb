using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data.Models;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Identity.Data;
using SGDb.Identity.Data.Models;
using SGDb.Identity.Models.Identity;
using SGDb.Identity.Services.Identity.Contracts;

namespace SGDb.Identity.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IdentityDbContext _dbContext;

        public IdentityService(SignInManager<User> signInManager, IdentityDbContext dbContext)
        {
            this._signInManager = signInManager;
            this._dbContext = dbContext;
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

        public async Task ChangePassword(string userId, ChangePasswordInputModel changePasswordInputModel)
        {
            var user = await this._signInManager.UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var result = await this._signInManager.UserManager.ChangePasswordAsync(user, changePasswordInputModel.CurrentPassword, changePasswordInputModel.NewPassword);

            if (!result.Succeeded)
                throw new ArgumentException(result.Errors.FirstOrDefault()?.Description ?? "Something went wrong.");
        }

        public async Task MarkAsPublished(string guidId)
        {
            var message = await this._dbContext.Messages.FirstOrDefaultAsync(m => m.GuidId == guidId);
            
            message?.MarkAsPublished();

            await this._dbContext.SaveChangesAsync();
        }
        
        public async Task Save(params Message[] messages)
        {
            messages.ForEach(m =>
            {
                if (!this._dbContext.Messages.Any(msg => msg.GuidId == m.GuidId))
                {
                    this._dbContext.Messages.Add(m);
                }
            });                
         
            await this._dbContext.SaveChangesAsync();
        }
    }
}