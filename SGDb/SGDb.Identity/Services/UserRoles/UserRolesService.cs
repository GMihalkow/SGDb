using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Identity.Data;
using SGDb.Identity.Models.UserRoles;
using SGDb.Identity.Services.Roles.Contracts;
using SGDb.Identity.Services.UserRoles.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGDb.Identity.Services.UserRoles
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IRolesService _rolesService;

        public UserRolesService(IdentityDbContext dbContext, IRolesService rolesService)
        {
            this._dbContext = dbContext;
            this._rolesService = rolesService;
        }

        public async Task<IEnumerable<UserRoleViewModel>> GetAllByUserIds(string[] userIds = null)
        {
            var userRoles = await this._dbContext.UserRoles
                .Where(ur => userIds.IsNullOrEmpty() || userIds.Contains(ur.UserId))
                .Select(ur => new UserRoleViewModel
                {
                    RoleId = ur.RoleId,
                    UserId = ur.UserId
                })
                .ToListAsync();

            var roles = await this._rolesService.GetAll(userRoles?.Select(ur => ur.RoleId)?.ToArray());

            roles.ForEach((r) =>
            {
                var userRole = userRoles.FirstOrDefault(ur => ur.RoleId == r.Id);
                
                if (userRole != null)
                {
                    userRole.RoleName = r.Name;
                }
            });

            return userRoles;
        }
    }
}