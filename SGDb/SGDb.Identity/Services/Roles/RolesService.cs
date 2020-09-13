using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Identity.Models.Roles;
using SGDb.Identity.Services.Roles.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGDb.Identity.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesService(RoleManager<IdentityRole> roleManager)
            => this._roleManager = roleManager;

        public async Task<RoleViewModel> Get(string id)
            => await this.BaseGet(r => r.Id == id);

        public async Task<RoleViewModel> GetByName(string name)
            => await this.BaseGet(r => r.Name == name);

        public async Task<IEnumerable<RoleViewModel>> GetAll(string[] ids = null) => await this._roleManager.Roles
                .Where(r => ids.IsNullOrEmpty() || ids.Contains(r.Id))
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

        public async Task Create(RoleInputModel model)
        {
            var roleEntity = new IdentityRole(model.Name);

            var result = await this._roleManager.CreateAsync(roleEntity);

            if (!result.Succeeded)
                throw new InvalidOperationException("Something went wrong.");
        }

        public async Task Edit(RoleEditModel model)
        {
            var roleEntity = await this._roleManager.FindByIdAsync(model.Id);

            if (roleEntity != null)
            {
                roleEntity.Name = model.Name;

                await this._roleManager.UpdateAsync(roleEntity);
            }
        }

        public async Task Delete(string id)
        {
            var roleEntity = await this._roleManager.FindByIdAsync(id);

            if (roleEntity != null)
               await this._roleManager.DeleteAsync(roleEntity);
        }

        private async Task<RoleViewModel> BaseGet(Expression<Func<IdentityRole, bool>> findRoleFunc)
        {
            var roleEntity = await this._roleManager.Roles.FirstOrDefaultAsync(findRoleFunc);

            if (roleEntity == null)
                return null;

            var roleViewModel = new RoleViewModel
            {
                Id = roleEntity.Id,
                Name = roleEntity.Name
            };

            return roleViewModel;
        }
    }
}