using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Common.Infrastructure.Attributes.Authorization;

namespace SGDb.Statistics.Controllers
{
    public class GameDetailsViewsController : BaseController
    {
        private readonly IGameDetailViewsService _gameDetailViewsService;

        public GameDetailsViewsController(IGameDetailViewsService gameDetailViewsService)
            => this._gameDetailViewsService = gameDetailViewsService;

        [Authorize]
        public async Task<IActionResult> GetCountByGameId(int id) =>
            this.Ok(await this._gameDetailViewsService.GetCountByGameId(id));

        [AuthorizeMultipleRoles(new [] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> GetCountByGameIds([FromQuery] IEnumerable<int> ids) =>
            this.Ok(await this._gameDetailViewsService.GetCountByGameIds(ids));
    }
}