namespace ShortRefs.Data.Mongo.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using ShortRefs.Data.Mongo.Models;
    using ShortRefs.Domain.Repositories;

    internal sealed class MongoSequenceCounterRepository : ISequenceCounterRepository
    {
        private readonly IMongoDatabase referenceDb;

        private readonly IMongoCollection<Counter> counters;

        public MongoSequenceCounterRepository(IMongoClient mongoClient)
        {
            if (mongoClient == null)
            {
                throw new ArgumentNullException(nameof(mongoClient));
            }

            this.referenceDb = mongoClient.GetDatabase("ReferenceDb");
            this.counters = this.referenceDb.GetCollection<Counter>("counters");
        }

        public async Task<bool> CreateIfNotExists(string sequenceId)
        {
            if (sequenceId == null)
            {
                throw new ArgumentNullException(nameof(sequenceId));
            }

            var tableExists = (await this.referenceDb.ListCollectionsAsync(
                new ListCollectionsOptions
                {
                    Filter = new BsonDocument("name", "counters")
                }))
                .Any();

            if (!tableExists)
            {
                await this.counters.InsertOneAsync(new Counter { Id = sequenceId, Count = 0 });
                return true;
            }

            return false;
        }

        public async Task<long> GetNextIdAsync(string sequenceId, CancellationToken cancellationToken)
        {
            if (sequenceId == null)
            {
                throw new ArgumentNullException(nameof(sequenceId));
            }

            var counter = await this.counters.FindOneAndUpdateAsync(
                              c => c.Id == sequenceId,
                              new BsonDocument("$inc", new BsonDocument { { "count", 1 } }),
                              cancellationToken: cancellationToken);

            if (counter == null)
            {
                throw new InvalidOperationException($"Not found sequence = '{sequenceId}'");
            }

            return counter.Count;
        }
    }
}