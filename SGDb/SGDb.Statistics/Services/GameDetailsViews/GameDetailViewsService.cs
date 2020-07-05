using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Statistics.Data;
using SGDb.Statistics.Data.Models;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;

namespace SGDb.Statistics.Services.GameDetailsViews
{
    public class GameDetailViewsService : IGameDetailViewsService
    {
        private readonly StatisticsDbContext _dbContext;

        public GameDetailViewsService(StatisticsDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<uint> GetCountByGameId(uint id) =>
            (uint) await this._dbContext.GameDetailsViews.CountAsync(g => g.GameId == id);


        public async Task Create(uint gameId, string userId)
        {
            await this._dbContext.GameDetailsViews.AddAsync(new GameDetailsView {GameId = gameId, UserId = userId});
            await this._dbContext.SaveChangesAsync();
        }
    }
}