using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace ShortRefs.Data.Mongo.Tests.ReferenceRepositoryTests
{
    using System;
    using System.Threading;

    using MongoDB.Driver;

    using ShortRefs.Data.Mongo.Repositories;
    using ShortRefs.Domain.Repositories;
    using ShortRefs.Domain.Services;

    public abstract class TestBase : IDisposable
    {
        protected readonly IReferenceRepository ReferenceRepository;
        protected readonly IReferenceEncoder ReferenceEncoder = new ReferenceEncoder();

        private readonly IMongoClient mongoClient;

        private long id;

        protected TestBase()
        {
            this.mongoClient = new MongoClient(Settings.MongoConnectionString);
            this.ReferenceRepository = new MongoReferenceRepository(this.mongoClient);
        }

        public void Dispose()
        {
            this.mongoClient.GetDatabase("ReferenceDb").DropCollection("references");
        }

        protected long NewId()
        {
            var result = this.id;

            Interlocked.Increment(ref this.id);

            return result;
        }
    }
}
