using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Common.Services.Common.Contracts;
using SGDb.Creators.Data;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Publishers;
using SGDb.Common.Services.Base.Contracts;

namespace SGDb.Creators.Services.Publishers.Contracts
{
    public interface IPublishersService : IBaseService<int, PublisherViewModel, Publisher, PublisherInputModel, PublisherEditModel>, IPersistMessages<CreatorsDbContext>
    {
        Task<PublisherViewModel> GetByName(string name);
        
        Task<IEnumerable<PublisherSearchViewModel>> GetSearchPublishers();
        
        Task<IEnumerable<BasicMultiselectOptionViewModel<int>>> GetAllPublishersForMultiselect();
    }
}