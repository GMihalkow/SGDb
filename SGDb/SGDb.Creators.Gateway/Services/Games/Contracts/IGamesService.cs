using Refit;
using SGDb.Creators.Gateway.Models.Games;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Creators.Gateway.Services.Games.Contracts
{
    public interface IGamesService
    {
        [Get("/api/Games/Get")]
        Task<GameDetailsViewModel> Get([Query]int id);

        [Get("/api/Games/GetAllSearchGames")]
        Task<IEnumerable<GameSearchViewModel>> GetAllSearchGames();
    }
}