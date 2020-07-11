using Microsoft.AspNetCore.Authorization;
using System;

namespace SGDb.Common.Infrastructure.Attributes.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class AuthorizeMultipleRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeMultipleRolesAttribute(string[] roles) => this.Roles = string.Join(", ", roles);
    }
}