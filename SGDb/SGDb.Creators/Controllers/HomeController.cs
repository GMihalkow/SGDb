using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;

namespace SGDb.Creators.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public Result Test()
        {
            return Result.Success();
        }
    }
}    