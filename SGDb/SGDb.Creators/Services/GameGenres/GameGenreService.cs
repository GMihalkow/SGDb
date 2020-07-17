using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.GameGenres;
using SGDb.Creators.Services.GameGenres.Contracts;
using SGDb.Creators.Services.Genres.Contracts;

namespace SGDb.Creators.Services.GameGenres
{
    public class GameGenreService : IGameGenreService
    {
        private readonly CreatorsDbContext _dbContext;
        private readonly IGenresService _genresService;

        public GameGenreService(CreatorsDbContext dbContext, IGenresService genresService)
        {
            this._dbContext = dbContext;
            this._genresService = genresService;
        }

        public async Task<IEnumerable<GameGenreViewModel>> GetByGameIds(IEnumerable<int> gameIds) =>
             await this._dbContext.GameGenres
                 .Where(ggnr => gameIds.IsNullOrEmpty() || gameIds.Contains(ggnr.GameId))
                 .Select(ggnr => new GameGenreViewModel
                 {
                     GameId = ggnr.GameId,
                     GenreId = ggnr.GenreId
                 })
                 .ToListAsync();

        public async Task BulkCreateByGameId(int gameId, IEnumerable<int> genreIds)
        {
            var genreEntities = await this._genresService.GetAll(genreIds?.ToArray());

            var gameGenreEntities = new List<GameGenre>();
            
            genreEntities.ForEach((ge) =>
            {
                gameGenreEntities.Add(new GameGenre { GameId = gameId, GenreId = ge.Id});
            });

            await this._dbContext.GameGenres.AddRangeAsync(gameGenreEntities);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteByGameId(int gameId)
        {
            var gameGenres = await this._dbContext.GameGenres.Where(ggnr => ggnr.GameId == gameId).ToListAsync();
            
            this._dbContext.GameGenres.RemoveRange(gameGenres);
            await this._dbContext.SaveChangesAsync();
        }
    }
}