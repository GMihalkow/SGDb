using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Common.Services.Common.Contracts;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Publishers;
using SGDb.Creators.Services.Base.Contracts;

namespace SGDb.Creators.Services.Publishers.Contracts
{
    public interface IPublishersService : IBaseService<uint, PublisherViewModel, Publisher, PublisherInputModel, PublisherEditModel>, IPersistMessages<CreatorsDbContext>
    {
        Task<PublisherViewModel> GetByName(string name);
        
        Task<IEnumerable<PublisherSearchViewModel>> GetSearchPublishers();
        
        Task<IEnumerable<BasicMultiselectOptionViewModel<uint>>> GetAllPublishersForMultiselect();
    }
}