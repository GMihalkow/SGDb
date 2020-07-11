using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Creators;
using SGDb.Creators.Services.Creators.Contracts;

namespace SGDb.Creators.Services.Creators
{
    public class CreatorsService : ICreatorsService
    {
        private readonly CreatorsDbContext _dbContext;

        public CreatorsService(CreatorsDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        
        public async Task<CreatorViewModel> Get(uint id)
        {
            var creatorEntity = await this._dbContext
                .Creators
                .Include(c => c.Games)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (creatorEntity == null) 
                return null;

            var creatorViewModel = new CreatorViewModel
            {
                Id = creatorEntity.Id,
                Username = creatorEntity.Username,
                CreatedOn = creatorEntity.CreatedOn,
                TotalGamesCreatedCount = creatorEntity.Games?.Count ?? 0
            };

            return creatorViewModel;
        }

        public async Task<IEnumerable<CreatorViewModel>> GetAll(IEnumerable<uint> ids = null)
        {
            var creatorsViewModels = await this._dbContext
                .Creators
                .Where(c => ids.IsNullOrEmpty() || ids.Contains(c.Id))
                .Select(c => new CreatorViewModel
                {
                    Id = c.Id,
                    Username = c.Username,
                    CreatedOn = c.CreatedOn,
                    TotalGamesCreatedCount = c.Games.IsNullOrEmpty() ? 0 : c.Games.Count
                })
                .ToListAsync();

            return creatorsViewModels;
        }
        
        public async Task<CreatorViewModel> GetByUserId(string userId)
        {
            var creatorEntity = await this._dbContext.Creators.FirstOrDefaultAsync(cr => cr.UserId == userId);

            if (creatorEntity == null)
                return null;

            var creatorViewModel = new CreatorViewModel
            {
                Id = creatorEntity.Id,
                Username = creatorEntity.Username,
                CreatedOn = creatorEntity.CreatedOn,
                TotalGamesCreatedCount = creatorEntity.Games?.Count ?? 0
            };

            return creatorViewModel;
        }
        
        public async Task Create(CreatorInputModel model)
        {
            var creatorEntity = new Creator
            {
                Username = model.Username,
                UserId = model.UserId
            };

            await this._dbContext.Creators.AddAsync(creatorEntity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Edit(CreatorEditModel model)
        {
            var creatorEntity = await this._dbContext.Creators.FirstOrDefaultAsync(cr => cr.Id == model.Id);

            if (creatorEntity == null)
            {
                throw new InvalidOperationException("Invalid creator id.");
            }
            
            creatorEntity.Username = model.Username;

            this._dbContext.Creators.Update(creatorEntity);
            await this._dbContext.SaveChangesAsync();
        }

        public Task Delete(uint id)
        {
            throw new System.NotImplementedException();
        }
    }
}