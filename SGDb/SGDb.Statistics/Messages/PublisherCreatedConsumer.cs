using System.Threading.Tasks;
using MassTransit;
using SGDb.Common.Messages.Creators;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Messages
{
    public class PublisherCreatedConsumer : IConsumer<PublisherCreatedMessage>
    {
        private readonly IStatisticsService _statisticsService;

        public PublisherCreatedConsumer(IStatisticsService statisticsService)
            => this._statisticsService = statisticsService;

        public async Task Consume(ConsumeContext<PublisherCreatedMessage> context)
            => await this._statisticsService.IncrementProperties(st => st.TotalPublishersCount);
    }
}