using Microsoft.AspNetCore.Mvc;

namespace SGDb.Common.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
    }
}