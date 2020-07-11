using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Models.GamePublishers;

namespace SGDb.Creators.Services.GamePublishers.Contracts
{
    public interface IGamePublishersService
    {
        Task<IEnumerable<GamePublisherViewModel>> GetByGameIds(IEnumerable<uint> gameIds);
        
        Task BulkCreateByGameId(uint gameId, IEnumerable<uint> publisherIds);

        Task DeleteByGameId(uint gameId);        
    }
}