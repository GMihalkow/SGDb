using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
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
        
        public async Task<IActionResult> GetAll()
        {
            var games = await this._gamesService.GetAll();
            
            return this.Ok(games);
        }

        public async Task<IActionResult> GetAllForAutoComplete()
        {
            var autoCompleteGames = await this._gamesService.GetAutoCompleteModels();

            return this.Ok(autoCompleteGames);
        }
    }
}