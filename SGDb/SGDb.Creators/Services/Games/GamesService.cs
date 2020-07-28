using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data.Models;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.GameGenres.Contracts;
using SGDb.Creators.Services.GamePublishers.Contracts;
using SGDb.Creators.Services.Games.Contracts;
using SGDb.Creators.Services.Genres.Contracts;
using SGDb.Creators.Services.Publishers.Contracts;

namespace SGDb.Creators.Services.Games
{
    public class GamesService : IGamesService
    {
        private readonly CreatorsDbContext _dbContext;
        private readonly IPublishersService _publishersService;
        private readonly IGenresService _genresService;
        private readonly IGameGenreService _gameGenreService;
        private readonly IGamePublishersService _gamePublishersService;

        public GamesService(CreatorsDbContext dbContext, IPublishersService publishersService,
            IGenresService genresService, IGameGenreService gameGenreService,
            IGamePublishersService gamePublishersService)
        {
            this._dbContext = dbContext;
            this._publishersService = publishersService;
            this._genresService = genresService;
            this._gameGenreService = gameGenreService;
            this._gamePublishersService = gamePublishersService;
        }

        public async Task<GameViewModel> Get(int id)
        {
            var gameEntity = await this._dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (gameEntity == null)
            {
                return null;
            }

            var gameViewModel = new GameViewModel
            {
                Name = gameEntity.Name,
                About = gameEntity.About,
                Price = gameEntity.Price,
                Recommendations = gameEntity.Recommendations,
                CreatorId = gameEntity.CreatorId,
                CreatedOn = gameEntity.CreatedOn,
                ReleasedOn = gameEntity.ReleasedOn,
                WebsiteUrl = gameEntity.WebsiteUrl,
                BackgroundImageUrl = gameEntity.BackgroundImageUrl,
                HeaderImageUrl = gameEntity.HeaderImageUrl
            };

            return gameViewModel;
        }

        public async Task<IEnumerable<GameViewModel>> GetAll(int[] ids = null)
        {
            var gameViewModels = await this._dbContext
                .Games
                .Where((g) => ids.IsNullOrEmpty() || ids.Contains(g.Id))
                .Select((g) => new GameViewModel
                {
                    Name = g.Name,
                    About = g.About,
                    Price = g.Price,
                    Recommendations = g.Recommendations,
                    CreatedOn = g.CreatedOn,
                    ReleasedOn = g.ReleasedOn,
                    WebsiteUrl = g.WebsiteUrl,
                    BackgroundImageUrl = g.BackgroundImageUrl,
                    HeaderImageUrl = g.HeaderImageUrl
                })
                .ToListAsync();

            return gameViewModels;
        }

        public async Task<IEnumerable<SimpleGameViewModel>> GetAllSimplifiedGameModels(int[] gameIds) => 
            await this._dbContext.Games
                .Where(g => gameIds.Contains(g.Id))
                .Select(g => new SimpleGameViewModel 
                {   Id = g.Id,
                    Name = g.Name 
                })
                .ToListAsync();

        public async Task<IEnumerable<GameAutoCompleteModel>> GetAutoCompleteGameModels()
        {
            var gameAutoCompleteModels = await this._dbContext
                .Games
                .Select(g => new GameAutoCompleteModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();

            return gameAutoCompleteModels;
        }

        public async Task<IEnumerable<GameIndexCardViewModel>> GetIndexGameCards()
        {
            var gameCardsModels = await this._dbContext
                .Games
                .OrderByDescending(g => g.CreatedOn)
                .Take(3)
                .Select(g => new GameIndexCardViewModel
                {
                    Id = g.Id,
                    Name = g.Name.Substring(0, g.Name.Length <= 15 ? g.Name.Length : 15) + "...",
                    HeaderUrl = g.HeaderImageUrl
                })
                .ToListAsync();

            return gameCardsModels;
        }

        public async Task<IEnumerable<FeaturedGameViewModel>> GetFeaturedGames()
        {
            var featuredGameModels = await this._dbContext.Games
                .Select(g => new FeaturedGameViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    PublisherNames = string.Join(", ", g.Publishers.Select(p => p.Publisher.Name)),
                    GenreNames = string.Join(", ", g.Genres.Select(p => p.Genre.Name)),
                    ReleasedOn = g.ReleasedOn,
                    HeaderImageUrl = g.HeaderImageUrl
                })
                .ToListAsync();

            return featuredGameModels;
        }

        public async Task<IEnumerable<GameSearchViewModel>> GetSearchGames()
        {
            var gameSearchViewModels = await this._dbContext.Games
                .Select(g => new GameSearchViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Price = g.Price,
                    Recommendations = g.Recommendations,
                    CreatorId = g.CreatorId,
                    ReleasedOn = g.ReleasedOn,
                    About = g.About,
                    WebsiteUrl = g.WebsiteUrl,
                    BackgroundImageUrl = g.BackgroundImageUrl,
                    HeaderImageUrl = g.HeaderImageUrl
                })
                .ToListAsync();

            var gameIds = gameSearchViewModels.Select(gsvm => gsvm.Id).ToList();
            var gameGenres = await this._gameGenreService.GetByGameIds(gameIds);
            var gamePublishers = await this._gamePublishersService.GetByGameIds(gameIds);
                
            gameSearchViewModels.ForEach(gsvm =>
            {
                gsvm.GenreIds = gameGenres.Where(ggnr => ggnr.GameId == gsvm.Id).Select(ggnr => ggnr.GenreId);
                gsvm.PublisherIds = gamePublishers.Where(gp => gp.GameId == gsvm.Id).Select(gp => gp.PublisherId);
            });
            
            return gameSearchViewModels;
        }

        public async Task Create(GameInputModel model)
        {
            var gameEntity = new Game
            {
                Name = model.Name,
                About = model.About,
                WebsiteUrl = model.WebsiteUrl,

                // TODO [GM]: Make non nullable?
                // TODO [GM]: Remove?
                Recommendations = 0,

                BackgroundImageUrl = model.BackgroundImageUrl,
                HeaderImageUrl = model.HeaderImageUrl,
                ReleasedOn = model.ReleasedOn,
                Price = model.Price,
                CreatorId = model.CreatorId
            };

            await this._dbContext.Games.AddAsync(gameEntity);
            await this._dbContext.SaveChangesAsync();

            var entry = this._dbContext.Entry(gameEntity).Entity;

            if (!model.PublisherIds.IsNullOrEmpty())
            {
                var validPublishers = await this._publishersService.GetAll(model.PublisherIds?.ToArray());
                var validPublisherIds = validPublishers.Select(p => p.Id);

                var gamePublishers = validPublisherIds
                    .Select(pId => new GamePublisher
                    {
                        GameId = entry.Id,
                        PublisherId = pId
                    });

                await this._dbContext.GamePublishers.AddRangeAsync(gamePublishers);
                await this._dbContext.SaveChangesAsync();
            }

            if (!model.GenreIds.IsNullOrEmpty())
            {
                var validGenres = await this._genresService.GetAll(model.GenreIds?.ToArray());
                var validGenreIds = validGenres.Select(g => g.Id);

                var gameGenres = validGenreIds
                    .Select(genreId => new GameGenre
                    {
                        GameId = entry.Id,
                        GenreId = genreId
                    });

                await this._dbContext.GameGenres.AddRangeAsync(gameGenres);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task Edit(GameEditModel model)
        {
            var gameEntity = await this._dbContext.Games.FirstOrDefaultAsync(g => g.Id == model.Id);

            if (gameEntity == null)
            {
                throw new InvalidOperationException("Invalid car id.");
            }

            gameEntity.Name = model.Name;
            gameEntity.About = model.About;
            gameEntity.Price = model.Price;
            gameEntity.ReleasedOn = model.ReleasedOn;
            gameEntity.WebsiteUrl = model.WebsiteUrl;
            gameEntity.HeaderImageUrl = model.HeaderImageUrl;
            gameEntity.BackgroundImageUrl = model.BackgroundImageUrl;

            await this._gameGenreService.DeleteByGameId(gameEntity.Id);
            await this._gameGenreService.BulkCreateByGameId(gameEntity.Id, model.GenreIds);

            await this._gamePublishersService.DeleteByGameId(gameEntity.Id);
            await this._gamePublishersService.BulkCreateByGameId(gameEntity.Id, model.PublisherIds);
        }

        public async Task Delete(int id)
        {
            var gameEntity = await this._dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (gameEntity != null)
            {
                this._dbContext.Remove(gameEntity);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task MarkAsPublished(string guidId) => await this._dbContext.MarkAsPublished(guidId);
        
        public async Task Save(params Message[] messages) => await this._dbContext.Save(messages);
    }
}