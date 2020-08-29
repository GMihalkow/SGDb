using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Identity.Services.UserRoles.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGDb.Identity.Controllers
{
    [Authorize(Roles = RolesConstants.Administrator)]
    public class UserRolesController : BaseController
    {
        private readonly IUserRolesService _userRolesService;

        public UserRolesController(IUserRolesService userRolesService)
            => this._userRolesService = userRolesService;

        [HttpPost]
        public async Task<IActionResult> GetByUserIds([FromBody] IEnumerable<string> userIds)
            => this.Ok(await this._userRolesService.GetAllByUserIds(userIds.ToArray()));
    }
}