using System.Threading.Tasks;
using Refit;

namespace SGDb.Creators.Gateway.Services.GameDetailsViewService.Contracts
{
    public interface IGameDetailsViewService
    {
        [Get("/api/GameDetailsViews/GetCountByGameId")]
        Task<uint> GetCountByGameId(uint id);
    }
}