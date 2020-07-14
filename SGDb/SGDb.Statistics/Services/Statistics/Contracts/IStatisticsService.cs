using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SGDb.Statistics.Models.Statistics;

namespace SGDb.Statistics.Services.Statistics.Contracts
{
    public interface IStatisticsService
    {
        Task<StatisticsViewModel> Get();

        Task IncrementProperties(params Expression<Func<StatisticsViewModel, object>>[] properties);

        Task DecrementProperties(params Expression<Func<StatisticsViewModel, object>>[] properties);
    }
}