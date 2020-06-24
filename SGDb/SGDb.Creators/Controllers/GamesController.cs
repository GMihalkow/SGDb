using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.Games.Contracts;

namespace SGDb.Creators.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            this._gamesService = gamesService;
        }

        public async Task<IActionResult> GetAll()
        {
            var games = await this._gamesService.GetAll();

            return this.Ok(Result<IEnumerable<GameViewModel>>.SuccessWith(games));
        }

        public async Task<IActionResult> GetAllForAutoComplete()
        {
            var autoCompleteGames = await this._gamesService.GetAutoCompleteModels();

            return this.Ok(Result<IEnumerable<GameAutoCompleteModel>>.SuccessWith(autoCompleteGames));
        }

        public async Task<IActionResult> GetGameIndexCards() =>
            this.Ok(Result<IEnumerable<GameIndexCardViewModel>>.SuccessWith(await this._gamesService.GetIndexGameCards()));
    }
}