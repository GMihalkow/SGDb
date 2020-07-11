using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Genres;

namespace SGDb.Creators.Services.Genres.Contracts
{
    public interface IGenresService
    {
        Task<IEnumerable<GenreViewModel>> GetAll(uint[] ids = null);
        
        Task<IEnumerable<BasicMultiselectOptionViewModel<uint>>> GetAllGenresForMultiselect();
    }
}