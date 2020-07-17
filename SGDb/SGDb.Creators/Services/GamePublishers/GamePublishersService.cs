using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.GamePublishers;
using SGDb.Creators.Services.GamePublishers.Contracts;
using SGDb.Creators.Services.Publishers.Contracts;

namespace SGDb.Creators.Services.GamePublishers
{
    public class GamePublishersService : IGamePublishersService
    {
        private readonly CreatorsDbContext _dbContext;
        private readonly IPublishersService _publishersService;

        public GamePublishersService(CreatorsDbContext dbContext, IPublishersService publishersService)
        {
            this._dbContext = dbContext;
            this._publishersService = publishersService;
        }

        public async Task<IEnumerable<GamePublisherViewModel>> GetByGameIds(IEnumerable<int> gameIds)
                => await this._dbContext.GamePublishers
                    .Where(gp => gameIds.IsNullOrEmpty() || gameIds.Contains(gp.GameId))
                    .Select(gp => new GamePublisherViewModel
                    {
                        GameId = gp.GameId,
                        PublisherId = gp.PublisherId
                    })
                    .ToListAsync();

        public async Task BulkCreateByGameId(int gameId, IEnumerable<int> publisherIds)
        {
            var publisherEntities = await this._publishersService.GetAll(publisherIds?.ToArray());

            var gamePublisherEntities = new List<GamePublisher>();
            
            publisherEntities.ForEach((ge) =>
            {
                gamePublisherEntities.Add(new GamePublisher { GameId = gameId, PublisherId = ge.Id});
            });

            await this._dbContext.GamePublishers.AddRangeAsync(gamePublisherEntities);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteByGameId(int gameId)
        {
            var gamePublishers = await this._dbContext.GamePublishers.Where(gp => gp.GameId == gameId).ToListAsync();
            
            this._dbContext.GamePublishers.RemoveRange(gamePublishers);
            await this._dbContext.SaveChangesAsync();
        }
    }
}