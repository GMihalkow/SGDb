using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SGDb.Common.Services.DataSeeder.Contracts;
using SGDb.Identity.Data;
using SGDb.Identity.Data.Models;
using SGDb.Identity.Services.DataSeeder;
using SGDb.Identity.Services.Identity;
using SGDb.Identity.Services.Identity.Contracts;
using SGDb.Identity.Services.Roles;
using SGDb.Identity.Services.Roles.Contracts;
using SGDb.Identity.Services.TokenGenerator;
using SGDb.Identity.Services.TokenGenerator.Contracts;
using SGDb.Identity.Services.UserRoles;
using SGDb.Identity.Services.UserRoles.Contracts;
using SGDb.Identity.Services.Users;
using SGDb.Identity.Services.Users.Contracts;

namespace SGDb.Identity.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    
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
                .AddTransient<IUsersService, UsersService>()
                .AddTransient<IUserRolesService, UserRolesService>()
                .AddTransient<IRolesService, RolesService>()
                .AddTransient<ITokenGeneratorService, TokenGeneratorService>()
                .AddTransient<IDataSeeder, IdentityDataSeeder>();
    }
}