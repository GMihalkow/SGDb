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
        private readonly UserManager<User> _userManager;
        private readonly IOptions<AppSettings> _options;

        public TokenGeneratorService(UserManager<User> userManager, IOptions<AppSettings> options)
        {
            this._userManager = userManager;
            this._options = options;
        }

        public async Task<string> GenerateToken(string username, string password)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Value.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}