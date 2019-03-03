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
            var id = this.NewId();
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));

            await this.ReferenceRepository.CreateAsync(reference, CancellationToken.None);

            var expected = reference;
            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var id = this.NewId();
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));

            await this.ReferenceRepository.CreateAsync(reference, CancellationToken.None);

            reference.IncrementRedirects();

            await this.ReferenceRepository.UpdateAsync(reference, CancellationToken.None);

            var expected = reference;
            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var id = this.NewId();
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x));

            await this.ReferenceRepository.CreateAsync(reference, CancellationToken.None);

            await this.ReferenceRepository.DeleteAsync(reference, CancellationToken.None);

            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);
            actual.Should().BeNull();
        }
    }
}
