using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Gateway.Models.Games;
using SGDb.Creators.Gateway.Services.GameDetailsViewService.Contracts;
using SGDb.Creators.Gateway.Services.Games.Contracts;

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
        public async Task<IActionResult> GetGameDetails(uint id)
        {
            var game = await this._gamesService.Get(id);
            game.Views = await this._gameDetailsViewService.GetCountByGameId(id);;
            
            return this.Ok(Result<GameDetailsViewModel>.SuccessWith(game));
        }
    }
}