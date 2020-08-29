using Refit;
using SGDb.Creators.Gateway.Models.UserRoles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Creators.Gateway.Services.UserRoles.Contracts
{
    public interface IUserRolesService
    {
        [Post("/api/UserRoles/GetByUserIds")]
        Task<IEnumerable<UserRoleViewModel>> GetByUserIds([Body] IEnumerable<string> userIds);
    }
}