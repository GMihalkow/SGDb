using System.Threading.Tasks;
using SGDb.Statistics.Models.Statistics;

namespace SGDb.Statistics.Services.Statistics.Contracts
{
    public interface IStatisticsService
    {
        Task<StatisticsViewModel> Get();

        Task IncrementGamesCount();
        
        Task DecrementGamesCount();
    }
}