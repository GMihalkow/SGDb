using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Identity.Models.Identity;
using SGDb.Identity.Services.TokenGenerator.Contracts;

namespace SGDb.Identity.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public IdentityController(ITokenGeneratorService tokenGeneratorService)
        {
            this._tokenGeneratorService = tokenGeneratorService;
        }
        
        [HttpPost]
        public async Task<Result<string>> Authenticate([FromForm] LoginModel loginModel)
        {
            // TODO [GM]: Finish implementation
            var token = await this._tokenGeneratorService.GenerateToken(loginModel.Username, loginModel.Password);
            
            return Result.SuccessWith(token);
        }
    }
}