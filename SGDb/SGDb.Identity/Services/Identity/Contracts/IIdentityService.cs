using System.Threading.Tasks;
using SGDb.Common.Data.Models;
using SGDb.Identity.Models.Identity;

namespace SGDb.Identity.Services.Identity.Contracts
{
    public interface IIdentityService
    {
        Task<string> GetUserRole(string email);
        
        Task Register(RegisterInputModel registerInputModel);

        Task<bool> Login(LoginInputModel loginInputModel);
        
        Task MarkAsPublished(string guidId);
        
        Task Save(params Message[] messages);
    }
}