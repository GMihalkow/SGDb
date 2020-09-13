using Microsoft.AspNetCore.Identity;
using SGDb.Common.Services.Base.Contracts;
using SGDb.Identity.Models.Roles;
using System.Threading.Tasks;

namespace SGDb.Identity.Services.Roles.Contracts
{
    public interface IRolesService : IBaseService<string, RoleViewModel, IdentityRole, RoleInputModel, RoleEditModel>
    {
        Task<RoleViewModel> GetByName(string name);
    }
}