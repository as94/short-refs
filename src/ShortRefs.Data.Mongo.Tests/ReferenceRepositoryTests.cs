namespace ShortRefs.Data.Mongo.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using MongoDB.Driver;

    using ShortRefs.Data.Mongo.Repositories;
    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Repositories;
    using ShortRefs.Domain.Services;

    using Xunit;

    public sealed class ReferenceRepositoryTests : IDisposable
    {
        public readonly IReferenceRepository ReferenceRepository;
        public readonly ISequenceCounterRepository SequenceCounterRepository;
        public readonly IReferenceEncoder ReferenceEncoder = new ReferenceEncoder();

        private readonly IMongoClient mongoClient;

        public ReferenceRepositoryTests()
        {
            this.mongoClient = new MongoClient(Settings.MongoConnectionString);
            this.SequenceCounterRepository = new MongoSequenceCounterRepository(this.mongoClient);
            this.ReferenceRepository = new MongoReferenceRepository(this.mongoClient);

            this.SequenceCounterRepository.CreateIfNotExistsAsync("referenceId").Wait();
        }
        [Fact]
        public async Task CreateTest()
        {
            var id = await this.NewIdAsync();
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());

            await this.ReferenceRepository.CreateAsync(reference, CancellationToken.None);

            var expected = reference;
            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var id = await this.NewIdAsync();
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());

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
            var id = await this.NewIdAsync();
            var reference = Reference.CreateNew(id, "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());

            await this.ReferenceRepository.CreateAsync(reference, CancellationToken.None);

            await this.ReferenceRepository.DeleteAsync(reference, CancellationToken.None);

            var actual = await this.ReferenceRepository.GetAsync(id, CancellationToken.None);
            actual.Should().BeNull();
        }

        [Fact]
        public async Task NotFoundTest()
        {
            var actual = await this.ReferenceRepository.GetAsync(0, CancellationToken.None);
            actual.Should().BeNull();
        }

        [Fact]
        public async Task FindTest()
        {
            var reference1 = Reference.CreateNew(await this.NewIdAsync(), "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference1, CancellationToken.None);

            var reference2 = Reference.CreateNew(await this.NewIdAsync(), "https://stackoverflow.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference2, CancellationToken.None);

            var reference3 = Reference.CreateNew(await this.NewIdAsync(), "https://www.youtube.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference3, CancellationToken.None);

            var expected = new[] { reference1, reference2, reference3 };
            var actual = await this.ReferenceRepository.FindAsync(new ReferenceQuery(), CancellationToken.None);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task FindByOriginalTest()
        {
            var reference1 = Reference.CreateNew(await this.NewIdAsync(), "https://docs.microsoft.com", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference1, CancellationToken.None);

            var reference2 = Reference.CreateNew(await this.NewIdAsync(), "https://stackoverflow.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
            await this.ReferenceRepository.CreateAsync(reference2, CancellationToken.None);

            var reference3 = Reference.CreateNew(await this.NewIdAsync(), "https://www.youtube.com/", x => this.ReferenceEncoder.Encode(x), Guid.NewGuid());
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

        public void Dispose()
        {
            this.mongoClient.GetDatabase("ReferenceDb").DropCollection("references");
            this.mongoClient.GetDatabase("ReferenceDb").DropCollection("counters");
        }

        private async Task<long> NewIdAsync()
        {
            return await this.SequenceCounterRepository.GetNextIdAsync("referenceId", CancellationToken.None);
        }
    }
}
