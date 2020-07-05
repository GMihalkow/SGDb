using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using SGDb.Creators.Gateway.Models.Creators;

namespace SGDb.Creators.Gateway.Services.Creators.Contracts
{
    public interface ICreatorsService
    {
        [Get("/api/Creators/GetAll")]
        Task<IEnumerable<CreatorViewModel>> GetAll();
        
        [Patch("/api/Creators/Edit")]
        Task Edit(CreatorEditModel editModel);
    }
}