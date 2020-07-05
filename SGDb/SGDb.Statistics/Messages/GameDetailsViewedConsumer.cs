using System.Threading.Tasks;
using MassTransit;
using SGDb.Common.Messages.Creators;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;

namespace SGDb.Statistics.Messages
{
    public class GameDetailsViewedConsumer : IConsumer<GameDetailsViewedMessage>
    {
        private readonly IGameDetailViewsService _gameDetailViewsService;

        public GameDetailsViewedConsumer(IGameDetailViewsService gameDetailViewsService)
        {
            this._gameDetailViewsService = gameDetailViewsService;
        }

        public async Task Consume(ConsumeContext<GameDetailsViewedMessage> context) =>
            await this._gameDetailViewsService.Create(context.Message.GameId, context.Message.UserId);
    }
}