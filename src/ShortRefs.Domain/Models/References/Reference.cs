namespace ShortRefs.Domain.Models.Users
{
    using System;

    public sealed class Reference
    {
        private Reference(int id, string original)
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

            this.Id = id;
            this.Original = original;
        }

        public static Reference CreateNew(int id, string original)
        {
            return new Reference(id, original);
        }

        public int Id { get; }

        public string Original { get; }

        public string Short => Encode(this.Id);

        private static string Encode(int id)
        {
            return string.Empty;
        }

        private static int Decode(string s)
        {
            return 0;
        }
    }
}
