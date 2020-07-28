using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SGDb.Common.Infrastructure.Attributes.Authorization;
using SGDb.Common.Infrastructure.Extensions;

namespace SGDb.Statistics.Controllers
{
    public class GameDetailsViewsController : BaseController
    {
        private readonly IGameDetailViewsService _gameDetailViewsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameDetailsViewsController(IGameDetailViewsService gameDetailViewsService, IHttpContextAccessor httpContextAccessor)
        {
            this._gameDetailViewsService = gameDetailViewsService;
            this._httpContextAccessor = httpContextAccessor;
        }

        // TODO [GM]: Remove UserId (if the property is not needed)?
        [Authorize]
        public async Task<IActionResult> GetMyViewedGamesHistory() =>
            this.Ok(await this._gameDetailViewsService.GetByUserId(this._httpContextAccessor.UserId()));

        [Authorize]
        public async Task<IActionResult> GetCountByGameId(int id) =>
            this.Ok(await this._gameDetailViewsService.GetCountByGameId(id));

        [AuthorizeMultipleRoles(new [] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> GetCountByGameIds([FromQuery] IEnumerable<int> ids) =>
            this.Ok(await this._gameDetailViewsService.GetCountByGameIds(ids));
    }
}