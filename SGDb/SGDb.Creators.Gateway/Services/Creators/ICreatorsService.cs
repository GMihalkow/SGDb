using Refit;
using SGDb.Creators.Gateway.Models.Creators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Creators.Gateway.Services.Creators
{
    public interface ICreatorsService
    {
        [Get("/api/Creators/GetAll")]
        Task<IEnumerable<CreatorViewModel>> GetAll();
    }
}