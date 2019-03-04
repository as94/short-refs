namespace ShortRefs.Domain.Models.References
{
    using System;

    public sealed class Reference
    {
        private Reference(long id, string original, string shortRef, long redirectsCount)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (!Uri.IsWellFormedUriString(original, UriKind.Absolute))
            {
                throw new ArgumentException("Bad uri format", original);
            }

            if (shortRef == null)
            {
                throw new ArgumentNullException(nameof(shortRef));
            }

            if (redirectsCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            this.Id = id;
            this.Original = original;
            this.Short = shortRef;
            this.RedirectsCount = redirectsCount;
        }

        public static Reference CreateNew(long newId, string original, Func<long, string> encodeFunc)
        {
            return new Reference(newId, original, encodeFunc(newId), 0);
        }

        public static Reference GetExisting(long id, string original, string shortRef, long redirectsCount)
        {
            return new Reference(id, original, shortRef, redirectsCount);
        }

        public long Id { get; }

        public string Original { get; }

        public string Short { get; }

        public long RedirectsCount { get; private set; }

        public void IncrementRedirects()
        {
            this.RedirectsCount++;
        }
    }
}
