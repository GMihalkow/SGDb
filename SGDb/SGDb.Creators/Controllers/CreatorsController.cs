using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Models.Creators;
using SGDb.Creators.Services.Creators.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Creators.Controllers
{
    [Authorize(Roles = RolesConstants.Administrator)]
    public class CreatorsController : BaseController
    {
        private readonly ICreatorsService _creatorsService;

        public CreatorsController(ICreatorsService creatorsService)
            => this._creatorsService = creatorsService;
        
        public async Task<IActionResult> GetAll()
        {
            // TODO [GM]: Add ids as a parameter?
            var creators = await this._creatorsService.GetAll();

            return this.Ok(Result<IEnumerable<CreatorViewModel>>.SuccessWith(creators));
        }
        
        [HttpPatch]
        public async Task<IActionResult> Edit(CreatorEditModel creatorEditModel)
        {
            try
            {
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