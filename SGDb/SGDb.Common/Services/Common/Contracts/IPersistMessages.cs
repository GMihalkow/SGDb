using System.Threading.Tasks;
using SGDb.Common.Data;
using SGDb.Common.Data.Models;

namespace SGDb.Common.Services.Common.Contracts
{
    public interface IPersistMessages<TMessageDbContext>
        where TMessageDbContext : MessageDbContext
    {
        Task MarkAsPublished(string guidId);
        
        Task Save(params Message[] messages);
    }
}