using Microsoft.Extensions.DependencyInjection;
using SGDb.Common.Services.DataSeeder.Contracts;
using SGDb.Statistics.Services.DataSeeder;

namespace SGDb.Statistics.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
            => services.AddTransient<IDataSeeder, StatisticsDataSeeder>();
    }
}