namespace ShortRefs.Domain.Models.Users
{
    using System;

    public sealed class User
    {
        public User(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
