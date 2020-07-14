using System.Threading.Tasks;
using MassTransit;
using SGDb.Common.Messages.Creators;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Messages
{
    public class GameCreatedConsumer : IConsumer<GameCreatedMessage>
    {
        private readonly IStatisticsService _statisticsService;

        public GameCreatedConsumer(IStatisticsService statisticsService)
            => this._statisticsService = statisticsService;

        public async Task Consume(ConsumeContext<GameCreatedMessage> context)
            => await this._statisticsService.IncrementProperties(st => st.TotalGamesCount);
    }
}