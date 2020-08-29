using SGDb.Identity.Models.UserRoles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Identity.Services.UserRoles.Contracts
{
    public interface IUserRolesService
    {
        Task<IEnumerable<UserRoleViewModel>> GetAllByUserIds(string[] userIds);
    }
}