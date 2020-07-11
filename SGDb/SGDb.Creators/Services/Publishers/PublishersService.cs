using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Publishers;
using SGDb.Creators.Services.Publishers.Contracts;

namespace SGDb.Creators.Services.Publishers
{
    public class PublishersService : IPublishersService
    {
        private readonly CreatorsDbContext _dbContext;

        public PublishersService(CreatorsDbContext dbContext) => this._dbContext = dbContext;

        public async Task<IEnumerable<PublisherViewModel>> GetAll(uint[] ids = null)
            => await this._dbContext
                .Publishers
                .Where(p => ids.IsNullOrEmpty() || ids.Contains(p.Id) == true)
                .Select(p => new PublisherViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedOn = p.CreatedOn,
                    CreatorId = p.CreatorId
                })
                .ToListAsync();

        public async Task<IEnumerable<BasicMultiselectOptionViewModel<uint>>> GetAllPublishersForMultiselect()
            => await this._dbContext
                .Publishers
                .Select(p => new BasicMultiselectOptionViewModel<uint>
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();
    }
}