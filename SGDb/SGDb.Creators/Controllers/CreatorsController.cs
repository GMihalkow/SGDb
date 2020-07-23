using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Models.Creators;
using SGDb.Creators.Services.Creators.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SGDb.Common.Infrastructure.Attributes.Authorization;
using SGDb.Common.Infrastructure.Extensions;

namespace SGDb.Creators.Controllers
{
    public class CreatorsController : BaseController
    {
        private readonly ICreatorsService _creatorsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatorsController(ICreatorsService creatorsService, IHttpContextAccessor httpContextAccessor)
        {
            this._creatorsService = creatorsService;
            this._httpContextAccessor = httpContextAccessor;
        }

        [AuthorizeMultipleRoles(new[] { RolesConstants.Administrator, RolesConstants.Creator })]
        public async Task<IActionResult> GetCurrentCreatorId()
        {
            var creatorViewModel = await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId());

            if (creatorViewModel == null)
                return this.NotFound(Result.Failure());

            return this.Ok(Result<int>.SuccessWith(creatorViewModel.Id));
        }

        [Authorize(Roles = RolesConstants.Administrator)]
        public async Task<IActionResult> GetAll()
        {
            // TODO [GM]: Add ids as a parameter?
            var creators = await this._creatorsService.GetAll();
                
            return this.Ok(Result<IEnumerable<CreatorViewModel>>.SuccessWith(creators));
        }
        
        [HttpPatch]
        [Authorize(Roles = RolesConstants.Administrator)]
        public async Task<IActionResult> Edit(CreatorEditModel creatorEditModel)
        {
            try
            {
                var currentUserCreator = await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId());

                if (creatorEditModel.Id == currentUserCreator?.Id)
                    throw new InvalidOperationException("You cannot edit yourself!");

                await this._creatorsService.Edit(creatorEditModel);
                
                return this.Ok(Result.Success());
            }
            catch (InvalidOperationException e)
            {
                return this.BadRequest(Result.Failure(e.Message));
            }
        }
    }
}