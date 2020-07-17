using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Common.Services.Common.Contracts;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.Base.Contracts;

namespace SGDb.Creators.Services.Games.Contracts
{
    public interface IGamesService : IBaseService<int, GameViewModel, Game, GameInputModel, GameEditModel>, IPersistMessages<CreatorsDbContext>
    {
        Task<IEnumerable<GameAutoCompleteModel>> GetAutoCompleteGameModels();

        Task<IEnumerable<GameIndexCardViewModel>> GetIndexGameCards();

        Task<IEnumerable<FeaturedGameViewModel>> GetFeaturedGames();

        Task<IEnumerable<GameSearchViewModel>> GetSearchGames();
    }
}