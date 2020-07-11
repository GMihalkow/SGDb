using System.Collections.Generic;
using System.Threading.Tasks;
using SGDb.Creators.Models.Common;
using SGDb.Creators.Models.Publishers;

namespace SGDb.Creators.Services.Publishers.Contracts
{
    public interface IPublishersService
    {
        Task<IEnumerable<PublisherViewModel>> GetAll(uint[] ids = null);
        
        Task<IEnumerable<BasicMultiselectOptionViewModel<uint>>> GetAllPublishersForMultiselect();
    }
}