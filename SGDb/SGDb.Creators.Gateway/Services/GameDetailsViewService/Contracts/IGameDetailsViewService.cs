using Refit;
using SGDb.Creators.Gateway.Models.Games;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Creators.Gateway.Services.GameDetailsViewService.Contracts
{
    public interface IGameDetailsViewService
    {
        [Get("/api/GameDetailsViews/GetCountByGameId")]
        Task<uint> GetCountByGameId(int id);

        [Get("/api/GameDetailsViews/GetCountByGameIds")]
        Task<IEnumerable<GameDetailsCountByIdViewModel>> GetCountByGameIds([Query(CollectionFormat.Multi)] IEnumerable<uint> ids);
    }
}