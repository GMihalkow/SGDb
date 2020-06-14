using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Creators.Services.Base.Contracts
{
    public interface IBaseService<TKey, TViewModel, TEntityModel, TInputModel, TEditModel>
    {
        Task<TViewModel> Get(TKey id);

        Task<IEnumerable<TViewModel>> GetAll(IEnumerable<TKey> ids = null);
        
        Task Create(TInputModel model);

        Task Edit(TEditModel model);

        Task Delete(TKey id);
    }
}