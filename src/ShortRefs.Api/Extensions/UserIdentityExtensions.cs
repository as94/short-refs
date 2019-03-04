namespace ShortRefs.Api.Extensions
{
    using System;
    using System.Security.Claims;

    using ShortRefs.Api.Exceptions;
    using ShortRefs.Api.Models;

    public static class UserIdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var identity = claimsPrincipal.Identity as UserIdentity;
            if (identity == null)
            {
                throw new ForbiddenException();
            }

            return identity.User.Id;
        }
    }
}
