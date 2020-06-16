using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SGDb.Common.Infrastructure.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static string UserId(this IHttpContextAccessor httpContextAccessor)
        {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("This request does not have an authenticated user.");
            }

            return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }
    }
}