using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Identity.Models.Identity;
using SGDb.Identity.Services.Identity.Contracts;
using SGDb.Identity.Services.TokenGenerator.Contracts;

namespace SGDb.Identity.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public IdentityController(ITokenGeneratorService tokenGeneratorService, IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            this._tokenGeneratorService = tokenGeneratorService;
            this._httpContextAccessor = httpContextAccessor;
            this._identityService = identityService;
        }

        [HttpPost]
        public async Task<Result<string>> Register([FromForm] RegisterInputModel registerInputModel)
        {
            try
            {
                await this._identityService.Register(registerInputModel);

                var token = await this._tokenGeneratorService.GenerateToken(registerInputModel.Username, registerInputModel.Password);
                
                return Result.SuccessWith(token);
            }
            catch (InvalidOperationException ex)
            {
                this.Response.StatusCode = 400;
                return Result.Failure(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<Result<string>> Login([FromForm] LoginInputModel loginInputInputModel)
        {
            var loginSucceeded = await this._identityService.Login(loginInputInputModel);

            if (!loginSucceeded)
            {
                // TODO [GM]: Sort this out
                this.Response.StatusCode = 400;
                return Result.Failure("Invalid password and username combination.");
            }

            var token = await this._tokenGeneratorService.GenerateToken(loginInputInputModel.Username, loginInputInputModel.Password);
            
            return Result.SuccessWith(token);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Test()
        {
            var loggedUserId = this._httpContextAccessor.UserId();
            
            return this.Ok();
        }
    }
}