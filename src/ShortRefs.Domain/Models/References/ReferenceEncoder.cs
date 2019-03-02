namespace ShortRefs.Domain.Models.References
{
    using System;
    using System.Text;

    // TODO: add tests
    // idea: https://stackoverflow.com/questions/742013/how-do-i-create-a-url-shortener/742047#742047
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

        public string Encode(int id)
        {
            if (id == 0)
            {
                return this.alphabet[0].ToString();
            }

            var sb = new StringBuilder();

            while (id > 0)
            {
                sb.Insert(0, this.alphabet[id % this.alphabetBase]);
                id /= this.alphabetBase;
            }

            return sb.ToString();
        }

        public int Decode(string str)
        {
            var id = 0;

            foreach (var c in str)
            {
                id = (id * this.alphabetBase) + this.alphabet.IndexOf(c);
            }

            return id;
        }
    }
}
