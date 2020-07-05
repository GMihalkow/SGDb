using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;

namespace SGDb.Statistics.Controllers
{
    public class GameDetailsViewsController : BaseController
    {
        private readonly IGameDetailViewsService _gameDetailViewsService;

        public GameDetailsViewsController(IGameDetailViewsService gameDetailViewsService)
        {
            this._gameDetailViewsService = gameDetailViewsService;
        }

        [Authorize]
        public async Task<IActionResult> GetCountByGameId(uint id) =>
            this.Ok(await this._gameDetailViewsService.GetCountByGameId(id));
    }
}