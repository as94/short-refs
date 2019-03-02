namespace ShortRefs.Api.Models
{
    using System;
    using System.Security.Principal;

    using ShortRefs.Domain.Models.Users;

    internal sealed class UserIdentity : IIdentity
    {
        public UserIdentity(User user, string authenticationType)
        {
            this.User = user ?? throw new ArgumentNullException(nameof(user));
            this.AuthenticationType = authenticationType ?? throw new ArgumentNullException(nameof(authenticationType));
        }

        public bool IsAuthenticated => true;

        public User User { get; }

        public string AuthenticationType { get; }

        public string Name => this.User.Id.ToString();
    }
}
