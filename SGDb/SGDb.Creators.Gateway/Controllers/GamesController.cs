using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Gateway.Models.Games;
using SGDb.Creators.Gateway.Services.GameDetailsViewService.Contracts;
using SGDb.Creators.Gateway.Services.Games.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGDb.Common.Infrastructure.Attributes.Authorization;

namespace SGDb.Creators.Gateway.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;
        private readonly IGameDetailsViewService _gameDetailsViewService;

        public GamesController(IGamesService gamesService, IGameDetailsViewService gameDetailsViewService)
        {
            this._gamesService = gamesService;
            this._gameDetailsViewService = gameDetailsViewService;
        }

        [Authorize]
        public async Task<IActionResult> GetGameDetails([FromQuery]int id)
        {
            var game = await this._gamesService.Get(id);
            
            if (game == null)
                return this.NotFound(Result.Failure("Game not found"));
            
            game.Views = await this._gameDetailsViewService.GetCountByGameId(id);;

            return this.Ok(Result<GameDetailsViewModel>.SuccessWith(game));
        }

        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> GetAllSearchGames()
        {
            var gamesSearchViewModels = (await this._gamesService.GetAllSearchGames()).ToList();
            var gamesDetailsViewsCountByGameIds = (await this._gameDetailsViewService
                    .GetCountByGameIds(gamesSearchViewModels
                        .Select(g => g.Id)))
                .ToList();

            gamesSearchViewModels.ForEach(gsvm =>
            {
                gsvm.Views = gamesDetailsViewsCountByGameIds.FirstOrDefault(gdv => gdv.GameId == gsvm.Id)?.Views ?? 0;
            });

            return this.Ok(Result<IEnumerable<GameSearchViewModel>>.SuccessWith(gamesSearchViewModels));
        }
    }
}