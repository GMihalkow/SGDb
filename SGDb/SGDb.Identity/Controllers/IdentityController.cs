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
        public async Task<IActionResult> Register([FromForm] RegisterInputModel registerInputModel)
        {
            try
            {
                await this._identityService.Register(registerInputModel);

                var token = await this._tokenGeneratorService.GenerateToken(registerInputModel.EmailAddress, registerInputModel.Password);
                
                return this.Ok(Result.SuccessWith(token));
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(Result.Failure(ex.Message));
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginInputModel loginInputInputModel)
        {
            try
            {
                var loginSucceeded = await this._identityService.Login(loginInputInputModel);

                if (!loginSucceeded)
                {
                    return this.BadRequest(Result.Failure("Invalid password and email combination."));
                }

                var token = await this._tokenGeneratorService.GenerateToken(loginInputInputModel.EmailAddress, loginInputInputModel.Password);
                
                return this.Ok(Result.SuccessWith(token));
            }
            catch (Exception)
            {
                return this.BadRequest(Result.Failure("Something went wrong."));
            }
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