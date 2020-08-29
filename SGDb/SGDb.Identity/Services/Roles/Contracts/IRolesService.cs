using SGDb.Identity.Models.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Identity.Services.Roles.Contracts
{
    public interface IRolesService
    {
        Task<IEnumerable<RoleViewModel>> GetAll(string[] ids);
    }
}