using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SGDb.Server.Data;
using SGDb.Server.Data.Models;

namespace SGDb.Server.Infrastructure.Extensions
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
                .AddEntityFrameworkStores<SGDbContext>();

            return services;
        }
    }
}