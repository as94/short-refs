namespace ShortRefs.Data.Mongo.Tests.ReferenceRepositoryTests
{
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using ShortRefs.Domain.Models.References;

    using Xunit;

    public sealed class ReadTests : TestBase
    {
        [Fact]
        public async Task NotFoundTest()
        {
            var actual = await this.ReferenceRepository.GetAsync(0, CancellationToken.None);
            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdTest()
        {
            var id = 0;
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));

            await this.ReferenceRepository.CreateAsync(reference);

            var expected = reference;
            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task FindTest()
        {
            var id1 = 0;
            var reference1 = Reference.CreateNew(id1, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));
            await this.ReferenceRepository.CreateAsync(reference1);

            var id2 = ++id1;
            var reference2 = Reference.CreateNew(id2, "https://stackoverflow.com/", x => this.ReferenceEncoder.Encode(x));
            await this.ReferenceRepository.CreateAsync(reference2);

            var id3 = ++id2;
            var reference3 = Reference.CreateNew(id3, "https://www.youtube.com/", x => this.ReferenceEncoder.Encode(x));
            await this.ReferenceRepository.CreateAsync(reference3);

            var expected = new[] { reference1, reference2, reference3 };
            var actual = await this.ReferenceRepository.FindAsync(new ReferenceQuery(), CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task FindByOriginalTest()
        {
            var id1 = 0;
            var reference1 = Reference.CreateNew(id1, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));
            await this.ReferenceRepository.CreateAsync(reference1);

            var id2 = ++id1;
            var reference2 = Reference.CreateNew(id2, "https://stackoverflow.com/", x => this.ReferenceEncoder.Encode(x));
            await this.ReferenceRepository.CreateAsync(reference2);

            var id3 = ++id2;
            var reference3 = Reference.CreateNew(id3, "https://www.youtube.com/", x => this.ReferenceEncoder.Encode(x));
            await this.ReferenceRepository.CreateAsync(reference3);

            var expected = reference2;
            var actual = await this.ReferenceRepository.FindAsync(
                             new ReferenceQuery("https://stackoverflow.com/"),
                             CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
