using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Models.Creators;
using SGDb.Creators.Services.Creators.Contracts;

namespace SGDb.Creators.Controllers
{
    [Authorize]
    public class CreatorsController : BaseController
    {
        private readonly ICreatorsService _creatorsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatorsController(ICreatorsService creatorsService, IHttpContextAccessor httpContextAccessor)
        {
            this._creatorsService = creatorsService;
            this._httpContextAccessor = httpContextAccessor;
        }
        
        [Authorize(Roles = RolesConstants.Administrator)]
        public async Task<IActionResult> GetAll()
        {
            // TODO [GM]: Add ids as a parameter?
            var creators = await this._creatorsService.GetAll();

            return this.Ok(creators);
            // return this.Ok(Result<IEnumerable<CreatorViewModel>>.SuccessWith(creators));
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatorInputModel creatorInputModel)
        {
            try
            {
                creatorInputModel.UserId = this._httpContextAccessor.UserId(); 
                
                await this._creatorsService.Create(creatorInputModel);

                return this.Ok(Result.Success());
            }
            catch (Exception)
            {
                return this.BadRequest(Result.Failure("Something went wrong when creating the Creator."));
            }
        }

        [HttpPatch]
        [Authorize(Roles = RolesConstants.Administrator)]
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