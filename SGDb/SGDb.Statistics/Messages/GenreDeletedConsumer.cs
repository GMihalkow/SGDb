using MassTransit;
using SGDb.Common.Messages.Creators;
using SGDb.Statistics.Services.Statistics.Contracts;
using System.Threading.Tasks;

namespace SGDb.Statistics.Messages
{
    public class GenreDeletedConsumer : IConsumer<GenreDeletedMessage>
    {
        private readonly IStatisticsService _statisticsService;

        public GenreDeletedConsumer(IStatisticsService statisticsService)
            => this._statisticsService = statisticsService;

        public async Task Consume(ConsumeContext<GenreDeletedMessage> context)
            => await this._statisticsService.DecrementProperties(st => st.TotalGenresCount);
    }
}