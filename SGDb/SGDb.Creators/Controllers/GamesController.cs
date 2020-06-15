using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.GamesService.Contracts;

namespace SGDb.Creators.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            this._gamesService = gamesService;
        }
        
        public async Task<Result<IEnumerable<GameViewModel>>> GetAll()
        {
            var games = await this._gamesService.GetAll();

            return Result<IEnumerable<GameViewModel>>.SuccessWith(games);
        }

        public async Task<Result<IEnumerable<GameAutoCompleteModel>>> GetAllForAutoComplete()
        {
            var autoCompleteGames = await this._gamesService.GetAutoCompleteModels();

            return Result<IEnumerable<GameAutoCompleteModel>>.SuccessWith(autoCompleteGames);
        }
    }
}