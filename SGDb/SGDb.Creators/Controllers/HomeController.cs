using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;

namespace SGDb.Creators.Controllers
{
    public class HomeController : BaseController
    {
        private readonly CreatorsDbContext _dbContext;
        private readonly UserManager<Creator> _userManager;

        public HomeController(CreatorsDbContext dbContext, UserManager<Creator> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Test()
        {
            return this.Ok("Success.");
        }
    }
}