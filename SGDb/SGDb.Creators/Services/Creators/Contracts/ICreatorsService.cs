using System.Threading.Tasks;
using SGDb.Creators.Data.Models;
using SGDb.Creators.Models.Creators;
using SGDb.Creators.Services.Base.Contracts;

namespace SGDb.Creators.Services.Creators.Contracts
{
    public interface ICreatorsService : IBaseService<uint, CreatorViewModel, Creator, CreatorInputModel, CreatorEditModel>
    {
        Task<CreatorViewModel> GetByUserId(string userId);
    }
}