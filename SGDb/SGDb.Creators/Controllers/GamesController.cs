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
using SGDb.Common.Data.Models;
using SGDb.Creators.Services.Creators.Contracts;
using SGDb.Creators.Services.GameGenres.Contracts;
using SGDb.Creators.Services.GamePublishers.Contracts;

namespace SGDb.Creators.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICreatorsService _creatorsService;
        private readonly IBus _bus;
        private readonly IGameGenreService _gameGenreService;
        private readonly IGamePublishersService _gamePublishersService;

        public GamesController(IGamesService gamesService, IHttpContextAccessor httpContextAccessor, ICreatorsService creatorsService, IBus bus, IGameGenreService gameGenreService, IGamePublishersService gamePublishersService)
        {
            this._gamesService = gamesService;
            this._httpContextAccessor = httpContextAccessor;
            this._creatorsService = creatorsService;
            this._bus = bus;
            this._gameGenreService = gameGenreService;
            this._gamePublishersService = gamePublishersService;
        }

        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var gameViewModel = await this._gamesService.Get(id);

            if (gameViewModel == null)
                return this.Ok(null);

            var message = new GameDetailsViewedMessage
            {
                GameId = id,
                UserId = this._httpContextAccessor.UserId()
            };

            var messageData = new Message(message);

            await this._gamesService.Save(messageData);
            await this._bus.PublishWithTimeout(message);
            await this._gamesService.MarkAsPublished(messageData.GuidId);
            
            return this.Ok(gameViewModel);  
        }

        [Authorize]
        public async Task<IActionResult> GetSimplifiedGamesByIds([FromQuery] int[] gameIds) => 
            this.Ok(await this._gamesService.GetAllSimplifiedGameModels(gameIds));

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
            
            var gameCreatedMessage = new GameCreatedMessage();
            var messageData = new Message(gameCreatedMessage);

            await this._gamesService.Save(messageData);
            await this._bus.PublishWithTimeout(gameCreatedMessage);
            await this._gamesService.MarkAsPublished(messageData.GuidId);
            
            return this.Ok(Result.Success());
        }
        
        [HttpPut]
        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> Edit([FromForm]GameEditModel gameEditModel)
        {
            var gameViewModel = await this._gamesService.Get(gameEditModel.Id);

            if (gameViewModel == null)
                return this.NotFound(Result.Failure());

            var currentCreator = await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId());

            if (gameViewModel.CreatorId != currentCreator?.Id && !this.User.IsInRole(RolesConstants.Administrator))
                return this.BadRequest(Result.Failure());
            
            await this._gamesService.Edit(gameEditModel);
            
            return this.Ok(Result.Success());
        }
        
        [HttpDelete]
        [AuthorizeMultipleRoles(new[] {RolesConstants.Administrator, RolesConstants.Creator})]
        public async Task<IActionResult> Delete(int id)
        {
            var gameViewModel = await this._gamesService.Get(id);

            if (gameViewModel == null) 
                return this.NotFound(Result.Failure());
            
            var currentCreator = await this._creatorsService.GetByUserId(this._httpContextAccessor.UserId());

            if (gameViewModel.CreatorId != currentCreator?.Id && !this.User.IsInRole(RolesConstants.Administrator))
                return this.BadRequest(Result.Failure());

            await this._gameGenreService.DeleteByGameId(id);
            await this._gamePublishersService.DeleteByGameId(id);
            await this._gamesService.Delete(id);

            var gameDeletedMessage = new GameDeletedMessage { GameId = id };
            var messageDate = new Message(gameDeletedMessage);
            
            await this._gamesService.Save(messageDate);
            await this._bus.PublishWithTimeout(gameDeletedMessage);
            await this._gamesService.MarkAsPublished(messageDate.GuidId);

            return this.Ok(Result.Success());
        } 
    }
}