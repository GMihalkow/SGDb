using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Models.GameGenres;

namespace SGDb.Creators.Services.GameGenres.Contracts
{
    public interface IGameGenreService
    {
        Task<IEnumerable<GameGenreViewModel>> GetByGameIds(IEnumerable<uint> gameIds);
        
        Task BulkCreateByGameId(uint gameId, IEnumerable<uint> genreIds);
        
        Task DeleteByGameId(uint gameId);
    }
}