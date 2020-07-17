using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Models.GameGenres;

namespace SGDb.Creators.Services.GameGenres.Contracts
{
    public interface IGameGenreService
    {
        Task<IEnumerable<GameGenreViewModel>> GetByGameIds(IEnumerable<int> gameIds);
        
        Task BulkCreateByGameId(int gameId, IEnumerable<int> genreIds);
        
        Task DeleteByGameId(int gameId);
    }
}