using System.Threading.Tasks;
using MassTransit;
using SGDb.Common.Messages.Creators;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Messages
{
    public class PublisherDeletedConsumer : IConsumer<PublisherDeletedMessage>
    {
        private readonly IStatisticsService _statisticsService;

        public PublisherDeletedConsumer(IStatisticsService statisticsService)
            => this._statisticsService = statisticsService;

        public async Task Consume(ConsumeContext<PublisherDeletedMessage> context)
            => await this._statisticsService.DecrementProperties(st => st.TotalPublishersCount);
    }
}