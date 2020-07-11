using MassTransit;
using SGDb.Common.Messages.Creators;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;
using System.Threading.Tasks;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Messages
{
    public class GameDeletedConsumer : IConsumer<GameDeletedMessage>
    {
        private readonly IGameDetailViewsService _gameDetailViewsService;
        private readonly IStatisticsService _statisticsService;

        public GameDeletedConsumer(IGameDetailViewsService gameDetailViewsService, IStatisticsService statisticsService)
        {
            this._gameDetailViewsService = gameDetailViewsService;
            this._statisticsService = statisticsService;
        }

        public async Task Consume(ConsumeContext<GameDeletedMessage> context)
        {
            await this._gameDetailViewsService.DeleteByGameId(context.Message.GameId);
            await this._statisticsService.DecrementGamesCount();
        }
    }
}