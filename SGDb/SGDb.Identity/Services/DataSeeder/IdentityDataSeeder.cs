using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SGDb.Common.Infrastructure;
using SGDb.Common.Services.DataSeeder.Contracts;

namespace SGDb.Identity.Services.DataSeeder
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityDataSeeder(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        public void SeedData()
        {
            if (!this._roleManager.Roles.Any())
            {
                Task.Run(async () =>
                    {
                        await this._roleManager.CreateAsync(new IdentityRole(RolesConstants.Administrator));
                        await this._roleManager.CreateAsync(new IdentityRole(RolesConstants.Creator));
                        await this._roleManager.CreateAsync(new IdentityRole(RolesConstants.User));
                    })
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}