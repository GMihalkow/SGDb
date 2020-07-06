using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Messages.Identity;
using SGDb.Identity.Models.Identity;
using SGDb.Identity.Services.Identity.Contracts;
using SGDb.Identity.Services.TokenGenerator.Contracts;
using SGDb.Identity.Services.Users.Contracts;
using System;
using System.Threading.Tasks;

namespace SGDb.Identity.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IIdentityService _identityService;
        private readonly IBus _bus;
        private readonly IUsersService _usersService;

        public IdentityController(ITokenGeneratorService tokenGeneratorService, IIdentityService identityService, IBus bus, IUsersService usersService)
        {
            this._tokenGeneratorService = tokenGeneratorService;
            this._identityService = identityService;
            this._bus = bus;
            this._usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterInputModel registerInputModel)
        {
            try
            {
                await this._identityService.Register(registerInputModel);

                var userId = await this._usersService.GetUserId(registerInputModel.EmailAddress);

                await this._bus.Publish(new UserCreatedMessage { Username = registerInputModel.Username, UserId = userId });

                var token = await this._tokenGeneratorService.GenerateToken(registerInputModel.EmailAddress, registerInputModel.Password);

                var roleName = await this._identityService.GetUserRole(registerInputModel.EmailAddress);

                var result = new AuthenticationSuccessOutputModel
                {
                    Role = roleName,
                    Token = token
                };
                
                return this.Ok(Result<AuthenticationSuccessOutputModel>.SuccessWith(result));
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

                var roleName = await this._identityService.GetUserRole(loginInputInputModel.EmailAddress);

                var result = new AuthenticationSuccessOutputModel
                {
                    Role = roleName,
                    Token = token
                };

                return this.Ok(Result<AuthenticationSuccessOutputModel>.SuccessWith(result));
            }
            catch (Exception)
            {
                return this.BadRequest(Result.Failure("Something went wrong."));
            }
        }
    }
}