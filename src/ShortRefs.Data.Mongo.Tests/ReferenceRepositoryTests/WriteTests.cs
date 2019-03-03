namespace ShortRefs.Data.Mongo.Tests.ReferenceRepositoryTests
{
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using ShortRefs.Domain.Models.References;

    using Xunit;

    public sealed class WriteTests : TestBase
    {
        [Fact]
        public async Task CreateTest()
        {
            var id = 0;
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));

            await this.ReferenceRepository.CreateAsync(reference);

            var expected = reference;
            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var id = 0;
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));

            await this.ReferenceRepository.CreateAsync(reference);

            reference.IncrementRedirects();

            await this.ReferenceRepository.UpdateAsync(reference);

            var expected = reference;
            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var id = 0;
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));

            await this.ReferenceRepository.CreateAsync(reference);

            await this.ReferenceRepository.DeleteAsync(reference);

            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);
            actual.Should().BeNull();
        }
    }
}
