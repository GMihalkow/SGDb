using SGDb.Common.Services.Common.Contracts;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Genres;
using SGDb.Creators.Services.Base.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Creators.Services.Genres.Contracts
{
    public interface IGenresService : IBaseService<int, GenreViewModel, Genre, GenreInputModel, GenreEditModel>, IPersistMessages<CreatorsDbContext>
    {
        Task<GenreViewModel> GetByName(string name);

        Task<IEnumerable<BasicMultiselectOptionViewModel<int>>> GetAllGenresForMultiselect();

        Task<IEnumerable<GenreSearchViewModel>> GetAllSearchGenres();
    }
}