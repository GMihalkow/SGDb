using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Identity.Data;
using SGDb.Identity.Models.Roles;
using SGDb.Identity.Services.Roles.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGDb.Identity.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly IdentityDbContext _dbContext;

        public RolesService(IdentityDbContext dbContext)
            => this._dbContext = dbContext;

        public async Task<IEnumerable<RoleViewModel>> GetAll(string[] ids = null) => await this._dbContext.Roles
                .Where(r => ids.IsNullOrEmpty() || ids.Contains(r.Id))
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();
    }
}