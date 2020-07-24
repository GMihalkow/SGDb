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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SGDb.Common.Data.Models;
using SGDb.Common.Infrastructure.Extensions;

namespace SGDb.Identity.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IIdentityService _identityService;
        private readonly IBus _bus;
        private readonly IUsersService _usersService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityController(ITokenGeneratorService tokenGeneratorService, IIdentityService identityService,
            IBus bus, IUsersService usersService, IHttpContextAccessor httpContextAccessor)
        {
            this._tokenGeneratorService = tokenGeneratorService;
            this._identityService = identityService;
            this._bus = bus;
            this._usersService = usersService;
            this._httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterInputModel registerInputModel)
        {
            try
            {
                await this._identityService.Register(registerInputModel);

                var userId = await this._usersService.GetUserId(registerInputModel.EmailAddress);

                var userCreatedMessage = new UserCreatedMessage
                    {Username = registerInputModel.Username, UserId = userId};
                var messageData = new Message(userCreatedMessage);

                await this._identityService.Save(messageData);
                await this._bus.PublishWithTimeout(userCreatedMessage);
                await this._identityService.MarkAsPublished(messageData.GuidId);

                var token = await this._tokenGeneratorService.GenerateToken(registerInputModel.EmailAddress,
                    registerInputModel.Password);

                var roleName = await this._identityService.GetUserRole(registerInputModel.EmailAddress);

                var result = new AuthenticationSuccessOutputModel
                {
                    Role = roleName,
                    Token = token
                };

                return this.Ok(Result<AuthenticationSuccessOutputModel>.SuccessWith(result));
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException || ex is InvalidOperationException)
                    return this.BadRequest(Result.Failure(ex.Message));

                throw;
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

                var token = await this._tokenGeneratorService.GenerateToken(loginInputInputModel.EmailAddress,
                    loginInputInputModel.Password);

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

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ChangePassword(ChangePasswordInputModel changePasswordInputModel)
        {
            try
            {
                await this._identityService
                    .ChangePassword(this._httpContextAccessor.UserId(), changePasswordInputModel);

                return this.Ok(Result.Success());
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException || e is ArgumentException)
                    return this.BadRequest(Result.Failure(e.Message));

                return this.BadRequest(Result.Failure());
            }
        }
    }
}