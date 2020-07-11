using SGDb.Statistics.Models.GameDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Statistics.Services.GameDetailsViews.Contracts
{
    public interface IGameDetailViewsService
    {
        Task Create(uint gameId, string userId);

        Task<uint> GetCountByGameId(uint id);

        Task<IEnumerable<GameDetailsCountByIdViewModel>> GetCountByGameIds(IEnumerable<uint> ids);

        Task DeleteByGameId(uint id);
    }
}