namespace ShortRefs.Domain.Models.Users
{
    using System;

    public sealed class Reference
    {
        private Reference(int id, string original, string shortRef, int redirectsCount, Guid ownerId)
        {
            if (id <= 0)
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

            if (redirectsCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            this.Id = id;
            this.Original = original;
            this.Short = shortRef;
            this.RedirectsCount = redirectsCount;
            this.OwnerId = ownerId;
        }

        public static Reference CreateNew(int id, string original, Func<int, string> encodeFunc, Guid ownerId)
        {
            return new Reference(id, original, encodeFunc(id), 0, ownerId);
        }

        public static Reference GetExisting(int id, string original, string shortRef, int redirectsCount, Guid ownerId)
        {
            return new Reference(id, original, shortRef, redirectsCount, ownerId);
        }

        public int Id { get; }

        public string Original { get; }

        public string Short { get; }

        public int RedirectsCount { get; private set; }

        public Guid OwnerId { get; }

        public void IncrementRedirects()
        {
            RedirectsCount++;
        }
    }
}
