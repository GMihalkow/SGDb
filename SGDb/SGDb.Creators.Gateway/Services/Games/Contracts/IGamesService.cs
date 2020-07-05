using System.Threading.Tasks;
using Refit;
using SGDb.Creators.Gateway.Models.Games;

namespace SGDb.Creators.Gateway.Services.Games.Contracts
{
    public interface IGamesService
    {
        [Get("/api/Games/Get")]
        Task<GameDetailsViewModel> Get([Query]uint id);
    }
}