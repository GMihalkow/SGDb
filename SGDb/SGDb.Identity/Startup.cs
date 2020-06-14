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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
            => services
                .AddApplicationSettings(this.Configuration)
                .AddWebService<IdentityDbContext>(this.Configuration)
                .AddIdentity()
                .AddApplicationServices();

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app.UseWebService(env);
    }
}