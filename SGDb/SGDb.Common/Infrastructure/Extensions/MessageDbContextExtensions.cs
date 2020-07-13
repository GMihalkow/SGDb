using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGDb.Common.Data;
using SGDb.Common.Data.Models;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class MessageDbContextExtensions
    {
        public static async Task MarkAsPublished(this MessageDbContext messageDbContext, string guidId)
        {
            var message = await messageDbContext.Messages.FirstOrDefaultAsync(m => m.GuidId == guidId);
            
            message?.MarkAsPublished();

            await messageDbContext.SaveChangesAsync();
        }

        public static async Task Save(this MessageDbContext messageDbContext, params Message[] messages)
        {
            messages.ForEach(m =>
            {
                if (!messageDbContext.Messages.Any(msg => msg.GuidId == m.GuidId))
                {
                    messageDbContext.Messages.Add(m);
                }
            });                
         
            await messageDbContext.SaveChangesAsync();
        }
    }
}