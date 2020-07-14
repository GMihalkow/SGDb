using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Statistics.Data;
using SGDb.Statistics.Models.Statistics;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly StatisticsDbContext _dbContext;

        public StatisticsService(StatisticsDbContext dbContext)
            => this._dbContext = dbContext;

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

        public async Task IncrementProperties(params Expression<Func<StatisticsViewModel, object>>[] properties)
        {
            var statistics = await this._dbContext.Statistics.FirstOrDefaultAsync();

            var members = properties
                .Select(e => ((UnaryExpression) e.Body).Operand.ToString())
                .Distinct()
                .ToList();
            
            members.ForEach(pName =>
            {
                if (pName.EndsWith("TotalGamesCount"))
                    statistics.TotalGamesCount++;
                else if (pName.EndsWith("TotalPublishersCount"))
                    statistics.TotalPublishersCount++;
                else if (pName.EndsWith("TotalPublishersCount"))
                    statistics.TotalGenresCount++;
            });

            await this._dbContext.SaveChangesAsync();
        }
        
        public async Task DecrementProperties(params Expression<Func<StatisticsViewModel,object>> [] properties)
        {
            var statistics = await this._dbContext.Statistics.FirstOrDefaultAsync();

            var members = properties
                .Select(e => ((UnaryExpression) e.Body).Operand.ToString())
                .Distinct();
            
            members.ForEach(pName =>
            {
                if (pName.EndsWith("TotalGamesCount"))
                    statistics.TotalGamesCount--;
                else if (pName.EndsWith("TotalPublishersCount"))
                    statistics.TotalPublishersCount--;
                else if (pName.EndsWith("TotalPublishersCount"))
                    statistics.TotalGenresCount--;
            });

            await this._dbContext.SaveChangesAsync();
        }
    }
}