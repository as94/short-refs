namespace ShortRefs.Domain.Models.Users
{
    using System;
    using System.Collections.Generic;

    public sealed class User
    {
        public User(Guid id)
        {
            this.Id = id;
        }

        public User(Guid id, IEnumerable<Reference> references) : this(id)
        {
            this.References = references ?? throw new ArgumentNullException(nameof(references));
        }

        public Guid Id { get; }

        public IEnumerable<Reference> References { get; } = Array.Empty<Reference>();
    }
}
