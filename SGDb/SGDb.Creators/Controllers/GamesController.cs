using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Attributes.Authorization;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Common.Messages.Creators;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.Games.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Services.Creators.Contracts;

namespace SGDb.Creators.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICreatorsService _creatorsService;
        private readonly IBus _bus;

        public GamesController(IGamesService gamesService, IHttpContextAccessor httpContextAccessor, ICreatorsService creatorsService, IBus bus)
        {
            this._gamesService = gamesService;
            this._httpContextAccessor = httpContextAccessor;
            this._creatorsService = creatorsService;
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

        [AuthorizeMultipleRoles(new[] { RolesConstants.Administrator, RolesConstants.Creator })]
        public async Task<IActionResult> GetAllSearchGames() => this.Ok(await this._gamesService.GetSearchGames());
    
        public async Task<IActionResult> GetGameIndexCards() =>
            this.Ok(Result<IEnumerable<GameIndexCardViewModel>>.SuccessWith(
                await this._gamesService.GetIndexGameCards()));

        [HttpPost]
        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> Create([FromForm]GameInputModel gameInputModel)
        {
            gameInputModel.CreatorId = (await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId()))?.Id ?? 0;

            if (gameInputModel.CreatorId == 0)
                return this.NotFound(Result.Failure()); 
            
            await this._gamesService.Create(gameInputModel);
            await this._bus.Publish(new GameCreatedMessage());
            
            return this.Ok(Result.Success());
        }
        
        [HttpPut]
        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> Edit([FromForm]GameEditModel gameEditModel)
        {
            var game = await this._gamesService.Get(gameEditModel.Id);

            if (game == null) 
                return this.NotFound(Result.Failure());
            
            await this._gamesService.Edit(gameEditModel);
            
            return this.Ok(Result.Success());
        }
        
        [HttpDelete]
        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> Delete(uint id)
        {
            var game = await this._gamesService.Get(id);

            if (game == null) 
                return this.NotFound(Result.Failure());

            await this._gamesService.Delete(id);
            await this._bus.Publish(new GameDeletedMessage { GameId = id });

            return this.Ok(Result.Success());
        } 
    }
}