namespace ShortRefs.Domain.Models.References
{
    using System;

    public sealed class ReferenceQuery
    {
        public ReferenceQuery(Guid? userId = null, string shortReference = null)
        {
            this.UserId = userId;
            this.ShortReference = shortReference;
        }

        public Guid? UserId { get; }

        public string ShortReference { get; }
    }
}
