using Microsoft.EntityFrameworkCore;
using SGDb.Statistics.Data;
using SGDb.Statistics.Data.Models;
using SGDb.Statistics.Models.GameDetails;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGDb.Statistics.Services.GameDetailsViews
{
    public class GameDetailViewsService : IGameDetailViewsService
    {
        private readonly StatisticsDbContext _dbContext;

        public GameDetailViewsService(StatisticsDbContext dbContext)
            => this._dbContext = dbContext;

        public async Task<IEnumerable<GameDetailsViewModel>> GetByUserId(string userId) => 
            await this._dbContext.GameDetailsViews.Where(gdv => gdv.UserId == userId)
                .Select(gdv => new GameDetailsViewModel
                {
                    CreatedOn = gdv.CreatedOn,
                    UserId = gdv.UserId,
                    GameId = gdv.GameId
                })
                .ToListAsync();

        public async Task<uint> GetCountByGameId(int id) =>
            (uint)await this._dbContext.GameDetailsViews.CountAsync(g => g.GameId == id);


        public async Task Create(int gameId, string userId)
        {
            await this._dbContext.GameDetailsViews.AddAsync(new GameDetailsView { GameId = gameId, UserId = userId });
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameDetailsCountByIdViewModel>> GetCountByGameIds(IEnumerable<int> ids)
        {
            var gameDetailsViewsCountByIds = await this._dbContext.GameDetailsViews
                .Select(gdv => new GameDetailsCountByIdViewModel
                {
                    GameId = gdv.GameId,
                    Views = (uint)this._dbContext.GameDetailsViews.Count(g => g.GameId == gdv.GameId)
                })
                .ToListAsync();

            return gameDetailsViewsCountByIds;
        }

        public async Task DeleteByGameId(int id)
        {
            var gameDetailsViews = await this._dbContext.GameDetailsViews.Where(gdv => gdv.GameId == id).ToListAsync();

            this._dbContext.GameDetailsViews.RemoveRange(gameDetailsViews);
            await this._dbContext.SaveChangesAsync();
        }
    }
}