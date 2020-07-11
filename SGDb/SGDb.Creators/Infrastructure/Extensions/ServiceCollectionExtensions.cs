using Microsoft.Extensions.DependencyInjection;
using SGDb.Creators.Services.Creators;
using SGDb.Creators.Services.Creators.Contracts;
using SGDb.Creators.Services.GameGenres;
using SGDb.Creators.Services.GameGenres.Contracts;
using SGDb.Creators.Services.GamePublishers;
using SGDb.Creators.Services.GamePublishers.Contracts;
using SGDb.Creators.Services.Games;
using SGDb.Creators.Services.Games.Contracts;
using SGDb.Creators.Services.Genres;
using SGDb.Creators.Services.Genres.Contracts;
using SGDb.Creators.Services.Publishers;
using SGDb.Creators.Services.Publishers.Contracts;

namespace SGDb.Creators.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services
                .AddTransient<ICreatorsService, CreatorsService>()
                .AddTransient<IGamesService, GamesService>()
                .AddTransient<IPublishersService, PublishersService>()
                .AddTransient<IGenresService, GenresService>()
                .AddTransient<IGameGenreService, GameGenreService>()
                .AddTransient<IGamePublishersService, GamePublishersService>();

    }
}