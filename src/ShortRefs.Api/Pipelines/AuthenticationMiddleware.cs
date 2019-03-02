namespace ShortRefs.Api.Pipelines
{
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using ShortRefs.Api.Models;
    using ShortRefs.Domain.Models.Users;

    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var identity = new UserIdentity(new User(Guid.NewGuid()), "Cookie");
            httpContext.User = new GenericPrincipal(identity, Array.Empty<string>());

            await this.next(httpContext);
        }
    }
}
