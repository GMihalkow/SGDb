using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Statistics.Data;
using SGDb.Statistics.Models.Statistics;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly StatisticsDbContext _dbContext;

        public StatisticsService(StatisticsDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        
        public async Task<StatisticsViewModel> Get()
        {
            var statisticsEntity = await this._dbContext.Statistics.FirstOrDefaultAsync();

            if (statisticsEntity == null)
                return null;

            var statisticsViewModel = new StatisticsViewModel
            {
                TotalGamesCount = statisticsEntity.TotalGamesCount,
                TotalGenresCount = statisticsEntity.TotalGenresCount,
                TotalPublishersCount = statisticsEntity.TotalPublishersCount
            };
            
            return statisticsViewModel;
        }
    }
}