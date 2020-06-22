using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SGDb.Identity.Data;
using SGDb.Identity.Data.Models;
using SGDb.Identity.Services.Identity;
using SGDb.Identity.Services.Identity.Contracts;
using SGDb.Identity.Services.TokenGenerator;
using SGDb.Identity.Services.TokenGenerator.Contracts;

namespace SGDb.Identity.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<IdentityDbContext>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
            services
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<ITokenGeneratorService, TokenGeneratorService>();
    }
}