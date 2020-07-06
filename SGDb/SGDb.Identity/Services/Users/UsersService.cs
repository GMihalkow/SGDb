using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGDb.Identity.Data.Models;
using SGDb.Identity.Services.Users.Contracts;
using System.Threading.Tasks;

namespace SGDb.Identity.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;

        public UsersService(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<string> GetUserId(string username)
        {
            var user = await this._userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);

            return user?.Id;
        }
    }
}