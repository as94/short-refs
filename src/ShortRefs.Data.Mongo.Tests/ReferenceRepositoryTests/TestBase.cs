namespace ShortRefs.Data.Mongo.Tests.ReferenceRepositoryTests
{
    using System;

    using MongoDB.Driver;

    using ShortRefs.Data.Mongo.Repositories;
    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Repositories;

    public abstract class TestBase : IDisposable
    {
        protected IReferenceRepository ReferenceRepository;
        protected readonly IReferenceEncoder ReferenceEncoder = new ReferenceEncoder();

        private readonly IMongoClient mongoClient;

        protected TestBase()
        {
            this.mongoClient = new MongoClient(Settings.MongoConnectionString);
            this.ReferenceRepository = new MongoReferenceRepository(this.mongoClient);
        }

        public void Dispose()
        {
            this.mongoClient.GetDatabase("ReferenceDb").DropCollection("references");
        }
    }
}
