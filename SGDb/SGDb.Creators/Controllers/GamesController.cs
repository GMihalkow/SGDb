using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Common.Messages.Creators;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.Games.Contracts;

namespace SGDb.Creators.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBus _bus;

        public GamesController(IGamesService gamesService, IHttpContextAccessor httpContextAccessor ,IBus bus)
        {
            this._gamesService = gamesService;
            this._httpContextAccessor = httpContextAccessor;
            this._bus = bus;
        }

        [Authorize]
        public async Task<IActionResult> Get(uint id)
        {
            var gameViewModel = await this._gamesService.Get(id);

            if (gameViewModel == null)
                return this.NotFound();

            var message = new GameDetailsViewedMessage
            {
                GameId = id,
                UserId = this._httpContextAccessor.UserId()
            };
            
            await this._bus.Publish(message);
            
            return this.Ok(gameViewModel);  
        }

        public async Task<IActionResult> GetAllForAutoComplete()
        {
            var autoCompleteGames = await this._gamesService.GetAutoCompleteGameModels();

            return this.Ok(Result<IEnumerable<GameAutoCompleteModel>>.SuccessWith(autoCompleteGames));
        }

        public async Task<IActionResult> GetAllFeatured()
        {
            var featuredGames = await this._gamesService.GetFeaturedGames();

            return this.Ok(Result<IEnumerable<FeaturedGameViewModel>>.SuccessWith(featuredGames));
        }

        public async Task<IActionResult> GetGameIndexCards() =>
            this.Ok(Result<IEnumerable<GameIndexCardViewModel>>.SuccessWith(
                await this._gamesService.GetIndexGameCards()));
    }
}