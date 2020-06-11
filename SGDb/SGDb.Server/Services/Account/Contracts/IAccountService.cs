using SGDb.Server.Data.Models;

namespace SGDb.Server.Services.Account.Contracts
{
    public interface IAccountService
    {
        string GenerateJwtToken(string username, string password);
    }
}