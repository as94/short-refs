namespace ShortRefs.Data.Mongo.Tests.ReferenceRepositoryTests
{
    using System;
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
            var id = this.NewId();
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());

            await this.ReferenceRepository.CreateAsync(reference, CancellationToken.None);

            var expected = reference;
            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task FindTest()
        {
            var reference1 = Reference.CreateNew(this.NewId(), "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference1, CancellationToken.None);
            
            var reference2 = Reference.CreateNew(this.NewId(), "https://stackoverflow.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference2, CancellationToken.None);
            
            var reference3 = Reference.CreateNew(this.NewId(), "https://www.youtube.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference3, CancellationToken.None);

            var expected = new[] { reference1, reference2, reference3 };
            var actual = await this.ReferenceRepository.FindAsync(new ReferenceQuery(), CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task FindByOriginalTest()
        {
            var reference1 = Reference.CreateNew(this.NewId(), "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference1, CancellationToken.None);
            
            var reference2 = Reference.CreateNew(this.NewId(), "https://stackoverflow.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference2, CancellationToken.None);
            
            var reference3 = Reference.CreateNew(this.NewId(), "https://www.youtube.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference3, CancellationToken.None);

            var expected = reference2;
            var actual = await this.ReferenceRepository.FindAsync(
                             new ReferenceQuery("https://stackoverflow.com/"),
                             CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CountTest()
        {
            var id1 = 0;
            var reference1 = Reference.CreateNew(id1, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference1, CancellationToken.None);

            var id2 = ++id1;
            var reference2 = Reference.CreateNew(id2, "https://stackoverflow.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference2, CancellationToken.None);

            var expected = 2;
            var actual = await this.ReferenceRepository.CountAsync(CancellationToken.None);

            actual.Should().Be(expected);
        }
    }
}
