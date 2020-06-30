using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SGDb.Common.Controllers;
using SGDb.Common.Infrastructure;
using SGDb.Statistics.Models.Statistics;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this._statisticsService = statisticsService;
        }

        public async Task<IActionResult> Get()
        {
            var statistics = await this._statisticsService.Get();

            return this.Ok(Result<StatisticsViewModel>.SuccessWith(statistics));
        }
    }
}