using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
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
        public IActionResult Authenticate([FromForm] LoginModel loginModel)
        {
            // TODO [GM]: Finish implementation
            var token = this._tokenGeneratorService.GenerateToken(loginModel.Username, loginModel.Password);
            
            return this.Ok(token);
        }
    }
}