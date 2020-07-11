using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Games;
using SGDb.Creators.Services.Base.Contracts;

namespace SGDb.Creators.Services.Games.Contracts
{
    public interface IGamesService : IBaseService<uint, GameViewModel, Game, GameInputModel, GameEditModel>
    {
        Task<IEnumerable<GameAutoCompleteModel>> GetAutoCompleteGameModels();

        Task<IEnumerable<GameIndexCardViewModel>> GetIndexGameCards();

        Task<IEnumerable<FeaturedGameViewModel>> GetFeaturedGames();

        Task<IEnumerable<GameSearchViewModel>> GetSearchGames();
    }
}