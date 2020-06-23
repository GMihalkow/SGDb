using System.Threading.Tasks;
using SGDb.Identity.Models.Identity;

namespace SGDb.Identity.Services.Identity.Contracts
{
    public interface IIdentityService
    {
        Task Register(RegisterInputModel registerInputModel);

        Task<bool> Login(LoginInputModel loginInputModel);
    }
}