using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SGDb.Watchdog
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            =>services.AddHealthChecksUI().AddInMemoryStorage();
        
        public void Configure(IApplicationBuilder app)
            => app.UseRouting()
                .UseEndpoints(endpoints => 
                    endpoints.MapHealthChecksUI());
    }
}