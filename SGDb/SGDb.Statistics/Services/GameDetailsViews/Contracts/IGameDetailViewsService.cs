using SGDb.Statistics.Models.GameDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Statistics.Services.GameDetailsViews.Contracts
{
    public interface IGameDetailViewsService
    {
        Task Create(int gameId, string userId);

        Task<uint> GetCountByGameId(int id);

        Task<IEnumerable<GameDetailsCountByIdViewModel>> GetCountByGameIds(IEnumerable<int> ids);

        Task DeleteByGameId(int id);
    }
}