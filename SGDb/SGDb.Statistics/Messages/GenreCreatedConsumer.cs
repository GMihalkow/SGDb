using MassTransit;
using SGDb.Common.Messages.Creators;
using SGDb.Statistics.Services.Statistics.Contracts;
using System.Threading.Tasks;

namespace SGDb.Statistics.Messages
{
    public class GenreCreatedConsumer : IConsumer<GenreCreatedMessage>
    {
        private readonly IStatisticsService _statisticsService;

        public GenreCreatedConsumer(IStatisticsService statisticsService)
            => this._statisticsService = statisticsService;

        public async Task Consume(ConsumeContext<GenreCreatedMessage> context)
            => await this._statisticsService.IncrementProperties(st => st.TotalGenresCount);
    }
}