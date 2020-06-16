using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGDb.Common.Infrastructure;
using SGDb.Identity.Data.Models;
using SGDb.Identity.Services.TokenGenerator.Contracts;

namespace SGDb.Identity.Services.TokenGenerator
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IOptions<AppSettings> _options;

        public TokenGeneratorService(SignInManager<User> signInManager, IOptions<AppSettings> options)
        {
            this._signInManager = signInManager;
            this._options = options;
        }

        public async Task<string> GenerateToken(string username, string password)
        {
            var signInResult = await this._signInManager.PasswordSignInAsync(username, password, true, false);

            if (!signInResult.Succeeded)
                return null;
            
            var user = await this._signInManager.UserManager.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return null;

            var key = Encoding.ASCII.GetBytes(_options.Value.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}