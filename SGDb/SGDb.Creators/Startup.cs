using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Creators.Data;
using SGDb.Creators.Infrastructure.Extensions;

namespace SGDb.Creators
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddWebService<CreatorsDbContext>(this.Configuration)
                .AddApplicationServices();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app.UseWebService(env);
    }
}