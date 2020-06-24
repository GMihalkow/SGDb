using Microsoft.Extensions.DependencyInjection;
using SGDb.Creators.Services.Creators;
using SGDb.Creators.Services.Creators.Contracts;
using SGDb.Creators.Services.Games;
using SGDb.Creators.Services.Games.Contracts;

namespace SGDb.Creators.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services
                .AddTransient<ICreatorsService, CreatorsService>()
                .AddTransient<IGamesService, GamesService>();
    }
}