using System.Threading.Tasks;

namespace SGDb.Identity.Services.Users.Contracts
{
    public interface IUsersService
    {
        Task<string> GetUserId(string username);
    }
}