
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ShortRefs.Data.Mongo.Tests")]

namespace ShortRefs.Domain.Services
{
    using System;
    using System.Text;

    // TODO: add tests
    // idea from here: https://stackoverflow.com/questions/742013/how-do-i-create-a-url-shortener/742047#742047
    internal sealed class ReferenceEncoder : IReferenceEncoder
    {
        public static readonly string DefaultAlphabet = "abcdefghijklmnopqrstuvwxyz0123456789";

        private readonly string alphabet;
        private readonly int alphabetBase;

        public ReferenceEncoder()
            : this(DefaultAlphabet)
        {
        }

        public ReferenceEncoder(string alphabet)
        {
            this.alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
            this.alphabetBase = this.alphabet.Length;
        }

        public string Encode(long id)
        {
            if (id == 0)
            {
                return this.alphabet[0].ToString();
            }

            var sb = new StringBuilder();

            while (id > 0)
            {
                long index = id % this.alphabetBase;
                sb.Insert(0, this.alphabet[(int)index]);
                id /= this.alphabetBase;
            }

            return sb.ToString();
        }

        public long Decode(string str)
        {
            long id = 0;

            foreach (var c in str)
            {
                id = (id * this.alphabetBase) + this.alphabet.IndexOf(c);
            }

            return id;
        }
    }
}
