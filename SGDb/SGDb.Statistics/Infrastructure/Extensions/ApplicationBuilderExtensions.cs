using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SGDb.Common.Services.DataSeeder.Contracts;

namespace SGDb.Statistics.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedDb(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var seeder = serviceProvider.GetService<IDataSeeder>();
            seeder.SeedData();
            
            return app;
        }
    }
}