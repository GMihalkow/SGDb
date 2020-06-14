using Microsoft.Extensions.DependencyInjection;
using SGDb.Creators.Services.GamesService;
using SGDb.Creators.Services.GamesService.Contracts;

namespace SGDb.Creators.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services
                .AddScoped<IGamesService, GamesService>();
    }
}