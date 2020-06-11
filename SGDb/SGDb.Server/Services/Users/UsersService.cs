using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SGDb.Server.Data.Models;
using SGDb.Server.Helpers;
using SGDb.Server.Services.Users.Contracts;

namespace SGDb.Server.Services.Users
{
    public class UsersService : IUsersService
    {
        // TODO [GM]: Finish implementation
        private List<User> _users = new List<User>
        { 
            new User { Id = Guid.NewGuid().ToString(), UserName = "John Doe" } 
        };

        private readonly AppSettings _appSettings;

        public UsersService(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.UserName == username);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}