namespace ShortRefs.Domain.Models.References
{
    using System;

    public sealed class ReferenceQuery
    {
        public ReferenceQuery(string originalReference = null)
        {
            this.OriginalReference = originalReference;
        }

        public string OriginalReference { get; }
    }
}
