namespace ShortRefs.Domain.Models.References
{
    using System;

    public sealed class ReferenceQuery
    {
        public ReferenceQuery(string originalReference = null, Guid? userId = null)
        {
            this.OriginalReference = originalReference;
            this.UserId = userId;
        }

        public string OriginalReference { get; }

        public Guid? UserId { get; }
    }
}
