using System;
using System.Linq;
using System.Net;
using Polly;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using static SGDb.Common.Infrastructure.WebConstants;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class HttpClientBuilderExtensions
    {
        public static void WithConfiguration(
            this IHttpClientBuilder httpClientBuilder,
            string baseAddress)
            => httpClientBuilder
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

                    client.BaseAddress = new Uri(baseAddress);

                    serviceProvider
                        .GetService<IHttpContextAccessor>()
                        ?.HttpContext
                        .Request
                        ?.Headers
                        .TryGetValue(AuthorizationHeaderName, out var tokenValues);

                    var token = tokenValues.ToString().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                        .LastOrDefault();

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return;
                    }

                    var authorizationHeader = new AuthenticationHeaderValue(AuthorizationHeaderValuePrefix, token);
                    client.DefaultRequestHeaders.Authorization = authorizationHeader;
                });

        // .AddTransientHttpErrorPolicy(policy => policy
        //     .OrResult(result => result.StatusCode == HttpStatusCode.NotFound)
        //     .WaitAndRetryAsync(6, retry => 
        //         TimeSpan.FromSeconds(Math.Pow(2, retry))))
        // .AddTransientHttpErrorPolicy(policy => policy
        //     .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
    }
}