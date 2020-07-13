using System;
using System.Text;
using GreenPipes;
using Hangfire;
using Hangfire.MySql.Core;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationSettings(this IServiceCollection services,
            IConfiguration configuration)
            => services
                .Configure<AppSettings>(configuration
                    .GetSection("ApplicationSettings"));

        public static IServiceCollection AddWebService<TDbContext>(this IServiceCollection services,
            IConfiguration configuration)
            where TDbContext : DbContext
        {
            services
                .AddScoped<DbContext, TDbContext>()
                .AddDbContext<TDbContext>(options
                    => options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                       mySqlOptionsAction: mySqlOption =>
                       {
                           mySqlOption.EnableRetryOnFailure(
                               maxRetryCount: 10,
                               maxRetryDelay: TimeSpan.FromSeconds(30),
                               errorNumbersToAdd: null);
                       }))
                .AddApplicationSettings(configuration)
                .AddJwtAuthentication(configuration)
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;

                    options.InvalidModelStateResponseFactory =
                        context => new BadRequestObjectResult((Result) context.ModelState);
                });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var secret = configuration
                .GetSection("ApplicationSettings")
                .GetValue<string>(nameof(AppSettings.Secret));

            var key = Encoding.ASCII.GetBytes(secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddHttpContextAccessor();

            return services;
        }
        
        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            IConfiguration configuration,
            params Type[] consumers)
        {
            services
                .AddMassTransit(mt =>
                {
                    consumers.ForEach(consumer => mt.AddConsumer(consumer));
        
                    mt.AddBus(bus => Bus.Factory.CreateUsingRabbitMq(rmq =>
                    {
                        rmq.Host("localhost");

                        consumers.ForEach(consumer => rmq.ReceiveEndpoint(consumer.FullName, endpoint =>
                        {
                            endpoint.PrefetchCount = 2;
                            endpoint.UseMessageRetry(x => x.Interval(5, 5000));
                            
                            endpoint.ConfigureConsumer(bus, consumer);
                        }));
                    }));
                })
                .AddMassTransitHostedService();

            services
                .AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(
                        new MySqlStorage(configuration.GetConnectionString("DefaultConnection"),
                        new MySqlStorageOptions { TablePrefix = "Hangfire" })));
            
            services.AddHangfireServer(opts=> opts.WorkerCount = 1);

            return services;
        }
    }
}