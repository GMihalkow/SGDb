using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Infrastructure;

namespace SGDb.Creators.Gateway.Areas.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = RolesConstants.Administrator)]
    [Route("api/[area]/[controller]/[action]")]
    public abstract class AdminController : ControllerBase
    {
    }
}