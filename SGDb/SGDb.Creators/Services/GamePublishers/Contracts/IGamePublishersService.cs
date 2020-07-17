using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Models.GamePublishers;

namespace SGDb.Creators.Services.GamePublishers.Contracts
{
    public interface IGamePublishersService
    {
        Task<IEnumerable<GamePublisherViewModel>> GetByGameIds(IEnumerable<int> gameIds);
        
        Task BulkCreateByGameId(int gameId, IEnumerable<int> publisherIds);

        Task DeleteByGameId(int gameId);        
    }
}