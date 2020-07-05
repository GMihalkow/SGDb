using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Creators.Data;
using SGDb.Creators.Infrastructure.Extensions;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.Games.Contracts;

namespace SGDb.Creators.Services.Games
{
    public class GamesService : IGamesService
    {
        private readonly CreatorsDbContext _dbContext;

        public GamesService(CreatorsDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<GameViewModel> Get(uint id)
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
                CreatedOn = gameEntity.CreatedOn,
                ReleasedOn = gameEntity.ReleasedOn,
                WebsiteUrl = gameEntity.WebsiteUrl,
                BackgroundImageUrl = gameEntity.BackgroundImageUrl,
                HeaderImageUrl = gameEntity.HeaderImageUrl
            };

            return gameViewModel;
        }

        public async Task<IEnumerable<GameViewModel>> GetAll(IEnumerable<uint> ids = null)
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
                    Id =  g.Id,
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

        public Task Create(GameInputModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Edit(GameEditModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task Delete(uint id)
        {
            var game = await this._dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game != null)
            {
                this._dbContext.Remove(game);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}