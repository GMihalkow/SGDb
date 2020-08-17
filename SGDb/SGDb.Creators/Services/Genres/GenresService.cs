using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data.Models;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Genres;
using SGDb.Creators.Services.Genres.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SGDb.Creators.Services.Genres
{
    public class GenresService : IGenresService
    {
        private readonly CreatorsDbContext _dbContext;

        public GenresService(CreatorsDbContext dbContext) => this._dbContext = dbContext;

        public async Task<GenreViewModel> Get(int id) => await this.BaseGet((g) => g.Id == id);

        public async Task<GenreViewModel> GetByName(string name) => await this.BaseGet((g) => g.Name == name);

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

        public async Task<IEnumerable<GenreSearchViewModel>> GetAllSearchGenres()
            => await this._dbContext.Genres.Select(g => new GenreSearchViewModel
            {
                Id = g.Id,
                Name = g.Name,
                CreatedOn = g.CreatedOn,
                CreatorId = g.CreatorId,
                CreatorName = g.Creator.Username
            })
            .ToListAsync();

        public async Task Create(GenreInputModel model)
        {
            var genreEntity = new Genre
            {
                CreatorId = model.CreatorId,
                Name = model.Name
            };

            await this._dbContext.Genres.AddAsync(genreEntity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Edit(GenreEditModel model)
        {
            var genreEntity = await this._dbContext.Genres.FirstOrDefaultAsync(g => g.Id == model.Id);

            if (genreEntity != null)
            {
                genreEntity.Name = model.Name;

                this._dbContext.Update(genreEntity);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var genreEntity = await this._dbContext.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genreEntity != null)
            {
                this._dbContext.Genres.Remove(genreEntity);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task MarkAsPublished(string guidId) => await this._dbContext.MarkAsPublished(guidId);

        public async Task Save(params Message[] messages) => await this._dbContext.Save(messages);

        private async Task<GenreViewModel> BaseGet(Expression<Func<Genre, bool>> findGenreFunc)
        {
            var genreEntity = await this._dbContext.Genres.FirstOrDefaultAsync(findGenreFunc);

            if (genreEntity == null)
                return null;

            var genreViewModel = new GenreViewModel
            {
                Id = genreEntity.Id,
                Name = genreEntity.Name,
                CreatedOn = genreEntity.CreatedOn,
                CreatorId = genreEntity.CreatorId
            };

            return genreViewModel;
        }
    }
}