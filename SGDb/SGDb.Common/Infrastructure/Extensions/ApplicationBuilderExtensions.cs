using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebService(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseHttpsRedirection()
                .UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod())
                .UseStatusCodePages(async (context) =>
                {
                    if (context.HttpContext.Response.StatusCode != 400)
                    {
                        context.HttpContext.Response.ContentType = "application/json";
                        await context
                            .HttpContext
                            .Response
                            .WriteAsync(JsonConvert
                                .SerializeObject(Result.Failure(),
                                    new JsonSerializerSettings
                                        {ContractResolver = new CamelCasePropertyNamesContractResolver()}));
                    }
                })
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints
                    => endpoints.MapControllers());

            return app;
        }
    }
}