using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGDb.Server.Services.Base.Contracts
{
    public interface IBaseService<TViewModel, TEntityModel, TInputModel, TEditModel>
    {
        Task<TViewModel> Get(string id);

        Task<IEnumerable<TViewModel>> GetAll();
        
        Task Create(TInputModel model);

        Task Edit(TEditModel model);

        Task Delete(string id);
    }
}