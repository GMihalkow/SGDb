using Microsoft.Extensions.DependencyInjection;
using SGDb.Common.Services.DataSeeder.Contracts;
using SGDb.Statistics.Services.DataSeeder;
using SGDb.Statistics.Services.GameDetailsViews;
using SGDb.Statistics.Services.GameDetailsViews.Contracts;
using SGDb.Statistics.Services.Statistics;
using SGDb.Statistics.Services.Statistics.Contracts;

namespace SGDb.Statistics.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
            => services
                .AddTransient<IDataSeeder, StatisticsDataSeeder>()
                .AddTransient<IStatisticsService, StatisticsService>()
                .AddTransient<IGameDetailViewsService, GameDetailViewsService>();

    }
}