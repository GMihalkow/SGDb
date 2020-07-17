using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Genres;
using SGDb.Creators.Services.Genres.Contracts;

namespace SGDb.Creators.Services.Genres
{
    public class GenresService : IGenresService
    {
        private readonly CreatorsDbContext _dbContext;

        public GenresService(CreatorsDbContext dbContext) => this._dbContext = dbContext;

        public async Task<IEnumerable<GenreViewModel>> GetAll(int[] ids = null)
            => await this._dbContext
                .Genres
                .Where(g => ids.IsNullOrEmpty() || ids.Contains(g.Id) == true)
                .Select(g => new GenreViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    CreatedOn = g.CreatedOn,
                    CreatorId = g.CreatorId
                })
                .ToListAsync();

        public async Task<IEnumerable<BasicMultiselectOptionViewModel<int>>> GetAllGenresForMultiselect()
            => await this._dbContext
                .Genres
                .Select(g => new BasicMultiselectOptionViewModel<int>
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();
    }
}