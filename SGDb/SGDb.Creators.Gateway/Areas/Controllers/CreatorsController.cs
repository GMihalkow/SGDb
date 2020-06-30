using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Gateway.Models.Creators;
using SGDb.Creators.Gateway.Services.Creators.Contracts;

namespace SGDb.Creators.Gateway.Areas.Controllers
{
    public class CreatorsController : AdminController
    {
        private readonly ICreatorsService _creatorsService;

        public CreatorsController(ICreatorsService creatorsService)
        {
            this._creatorsService = creatorsService;
        }
        
        public async Task<IActionResult> GetAll()
        {
            var creators = await this._creatorsService.GetAll();
            
            return this.Ok(Result<IEnumerable<CreatorViewModel>>.SuccessWith(creators));
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(CreatorEditModel editModel)
        {
            try
            {
                await this._creatorsService.Edit(editModel);
                
                return this.Ok(Result.Success());
            }
            catch (InvalidOperationException e)
            {
                return this.BadRequest(Result.Failure(e.Message));
            }
        }
    }
}