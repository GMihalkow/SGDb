using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Identity.Models.Roles;
using SGDb.Identity.Services.Roles.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Identity.Controllers
{
    [Authorize(Roles = RolesConstants.Administrator)]
    public class RolesController : BaseController
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
            => this._rolesService = rolesService;

        public async Task<IActionResult> GetAll()
        {
            var roles = await this._rolesService.GetAll();

            return this.Ok(Result<IEnumerable<RoleViewModel>>.SuccessWith(roles));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RoleInputModel roleInputModel)
        {
            var roleViewModel = await this._rolesService.GetByName(roleInputModel.Name);

            if (roleViewModel != null)
                return this.BadRequest(Result.Failure("Role already exists."));

            try
            {
                await this._rolesService.Create(roleInputModel);

                return this.Ok(Result.Success());
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(Result.Failure(ex.Message));
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(RoleEditModel roleEditModel)
        {
            var roleById = await this._rolesService.Get(roleEditModel.Id);

            if (roleById == null)
                return this.NotFound(Result.Failure("Role not found."));

            var roleByName = await this._rolesService.GetByName(roleEditModel.Name);

            if (roleByName != null && roleByName.Id != roleEditModel.Id)
                return this.BadRequest(Result.Failure("Role with this name already exists."));

            await this._rolesService.Edit(roleEditModel);

            return this.Ok(Result.Success());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var roleViewModel = await this._rolesService.Get(id);

                if (roleViewModel == null)
                    return this.NotFound(Result.Failure("Role not found."));

                await this._rolesService.Delete(id);

                // TODO [GM]: what to do with the users attached to the role?

                return this.Ok(Result.Success());
            }
            catch (Exception)
            {
                return this.BadRequest(Result.Failure("Something went wrong."));
            }
        }
    }
}