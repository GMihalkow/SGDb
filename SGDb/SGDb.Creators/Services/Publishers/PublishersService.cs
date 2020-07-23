using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data.Models;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Publishers;
using SGDb.Creators.Services.Publishers.Contracts;

namespace SGDb.Creators.Services.Publishers
{
    public class PublishersService : IPublishersService
    {
        private readonly CreatorsDbContext _dbContext;

        public PublishersService(CreatorsDbContext dbContext) => this._dbContext = dbContext;

        public async Task<IEnumerable<PublisherViewModel>> GetAll(int[] ids = null)
            => await this._dbContext
                .Publishers
                .Where(p => ids.IsNullOrEmpty() || ids.Contains(p.Id) == true)
                .Select(p => new PublisherViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreatedOn = p.CreatedOn,
                    CreatorId = p.CreatorId
                })
                .ToListAsync();

        public async Task<PublisherViewModel> GetByName(string name)
        {
            var publisherEntity = await this._dbContext
                .Publishers
                .SingleOrDefaultAsync(p => p.Name == name);

            if (publisherEntity == null)
                return null;

            var publisherViewModel = new PublisherViewModel
            {
                Id = publisherEntity.Id,
                Name = publisherEntity.Name,
                CreatedOn = publisherEntity.CreatedOn,
                CreatorId = publisherEntity.CreatorId
            };

            return publisherViewModel;
        }

        public async Task<IEnumerable<PublisherSearchViewModel>> GetSearchPublishers()
            => await this._dbContext
                .Publishers
                .Select(p => new PublisherSearchViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreatedOn = p.CreatedOn,
                    CreatorId =  p.CreatorId,
                    CreatorName = p.Creator.Username
                })
                .ToListAsync();

        public async Task<IEnumerable<BasicMultiselectOptionViewModel<int>>> GetAllPublishersForMultiselect()
            => await this._dbContext
                .Publishers
                .Select(p => new BasicMultiselectOptionViewModel<int>
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();

        public async Task<PublisherViewModel> Get(int id)
        {
            var publisherEntity = await this._dbContext
                .Publishers
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publisherEntity == null)
                return null;

            var publisherViewModel = new PublisherViewModel
            {
                Id = publisherEntity.Id,
                Name = publisherEntity.Name,
                CreatedOn = publisherEntity.CreatedOn,
                CreatorId = publisherEntity.CreatorId
            };

            return publisherViewModel;
        }
        
        public async Task Create(PublisherInputModel model)
        {
            var publisherEntity = new Publisher
            {
                CreatorId = model.CreatorId, 
                Name = model.Name
            };

            await this._dbContext.Publishers.AddAsync(publisherEntity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Edit(PublisherEditModel model)
        {
            var publisherEntity = await this._dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == model.Id);

            publisherEntity.Name = model.Name;

            await this._dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var publisherEntity = await this._dbContext.Publishers.FirstOrDefaultAsync(p => p.Id == id);

            if (publisherEntity != null)
            {
                this._dbContext.Publishers.Remove(publisherEntity);
                await this._dbContext.SaveChangesAsync();
            }
        }
        
        public async Task MarkAsPublished(string guidId) => await this._dbContext.MarkAsPublished(guidId);
        
        public async Task Save(params Message[] messages) => await this._dbContext.Save(messages);
    }
}