using System;
using System.Text;
using GreenPipes;
using Hangfire;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SGDb.Common.Messages;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var healthChecks = services.AddHealthChecks();
            
            healthChecks.AddSqlServer(configuration.GetConnectionString("DefaultConnection"));
            healthChecks.AddRabbitMQ(rabbitConnectionString: "amqp://rabbitmq:rabbitmq@rabbitmq:5672/");
            
            return services;
        }        
        
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
                    => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                       sqlServerOption =>
                       {
                           sqlServerOption.EnableRetryOnFailure(
                               maxRetryCount: 10,
                               maxRetryDelay: TimeSpan.FromSeconds(30),
                               errorNumbersToAdd: null);
                       }))
                .AddApplicationSettings(configuration)
                .AddJwtAuthentication(configuration)
                .AddHealth(configuration)
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
                        rmq.Host("rabbitmq", config =>
                        {
                            config.Username("rabbitmq");
                            config.Password("rabbitmq");
                        });

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
                    .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DbContext>();

                dbContext.Database.Migrate();
            }
            
            services.AddHangfireServer(opts=> opts.WorkerCount = 1);
            services.AddHostedService<MessagesHostedService>();

            return services;
        }
    }
}