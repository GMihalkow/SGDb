using System.Linq;
using System.Threading.Tasks;
using SGDb.Common.Services.DataSeeder.Contracts;
using SGDb.Statistics.Data;

namespace SGDb.Statistics.Services.DataSeeder
{
    public class StatisticsDataSeeder : IDataSeeder
    {
        private readonly StatisticsDbContext _dbContext;

        public StatisticsDataSeeder(StatisticsDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void SeedData()
        {
            if (!this._dbContext.Statistics.Any())
            {
                Task.Run(async () =>
                    {
                        await this._dbContext.Statistics.AddAsync(new Data.Models.Statistics());
                        await this._dbContext.SaveChangesAsync();
                    })
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}