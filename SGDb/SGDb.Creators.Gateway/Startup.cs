using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SGDb.Common.Infrastructure;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Gateway.Infrastructure;
using SGDb.Creators.Gateway.Services.GameDetailsViewService.Contracts;
using SGDb.Creators.Gateway.Services.Games.Contracts;

namespace SGDb.Creators.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddJwtAuthentication(this.Configuration)
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory =
                        context => new BadRequestObjectResult((Result) context.ModelState);
                });

            var serviceEndpoints = this.Configuration.GetSection(nameof(ServiceEndpoints))
                .Get<ServiceEndpoints>(config => config.BindNonPublicProperties = true);

            services
                .AddRefitClient<IGamesService>()
                .WithConfiguration(serviceEndpoints.Creators);
            
            services
                .AddRefitClient<IGameDetailsViewService>()
                .WithConfiguration(serviceEndpoints.Statistics);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app.UseWebService(env);
    }
}