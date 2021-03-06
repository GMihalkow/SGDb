using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Statistics.Data;
using SGDb.Statistics.Infrastructure.Extensions;
using SGDb.Statistics.Messages;

namespace SGDb.Statistics
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddWebService<StatisticsDbContext>(this.Configuration)
                .AddApplicationServices()
                .AddMessaging(this.Configuration, 
                    typeof(GameDetailsViewedConsumer),
                    typeof(GameDeletedConsumer),
                    typeof(GameCreatedConsumer),
                    typeof(PublisherDeletedConsumer),
                    typeof(PublisherCreatedConsumer),
                    typeof(GenreCreatedConsumer),
                    typeof(GenreDeletedConsumer));

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env)
                .Initialize()
                .SeedDb();
    }
}