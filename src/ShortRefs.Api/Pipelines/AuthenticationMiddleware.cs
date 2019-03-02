namespace ShortRefs.Api.Pipelines
{
    using System;
    using System.Net;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using ShortRefs.Api.Models;
    using ShortRefs.Domain.Models.Users;

    internal sealed class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            User user;
            if (!httpContext.Request.Cookies.TryGetValue("UserIdCookie", out var userId))
            {
                var newUserId = Guid.NewGuid();
                userId = newUserId.ToString();
                user = new User(newUserId);
            }
            else
            {
                if (!Guid.TryParse(userId, out var existingUserId))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                user = new User(existingUserId);
            }

            var identity = new UserIdentity(user, "Cookie");
            httpContext.User = new GenericPrincipal(identity, Array.Empty<string>());

            httpContext.Response.Cookies.Append(
                "UserIdCookie",
                userId,
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMonths(1)
                });

            await this.next(httpContext);
        }
    }
}
