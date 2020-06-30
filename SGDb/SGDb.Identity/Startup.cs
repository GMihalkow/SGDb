using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGDb.Common.Infrastructure.Extensions;
using SGDb.Identity.Data;
using SGDb.Identity.Infrastructure.Extensions;

namespace SGDb.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddIdentity()
                .AddWebService<IdentityDbContext>(this.Configuration)
                .AddApplicationServices();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseWebService(env)
                .SeedDb();
    }
}