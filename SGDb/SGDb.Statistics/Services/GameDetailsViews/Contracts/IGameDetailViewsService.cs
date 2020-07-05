using System.Threading.Tasks;

namespace SGDb.Statistics.Services.GameDetailsViews.Contracts
{
    public interface IGameDetailViewsService
    {
        Task Create(uint gameId, string userId);

        Task<uint> GetCountByGameId(uint id);
    }
}