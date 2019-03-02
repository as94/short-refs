namespace ShortRefs.Data.Mongo.Repositories
{
    using System;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Models.Users;
    using ShortRefs.Domain.Repositories;

    // TODO: add tests
    internal sealed class MongoReferenceRepository : IReferenceRepository
    {
        private readonly IMongoCollection<Models.Reference> references;

        public MongoReferenceRepository(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ReferenceDb");
            this.references = database.GetCollection<Models.Reference>("references");

            this.CreateIndexes();
        }

        public async Task<Reference> GetAsync(int id)
        {
            var model = await this.references.Find(new BsonDocument("_id", new ObjectId(id.ToString()))).FirstOrDefaultAsync();

            if (model == null)
            {
                return null;
            }

            return Reference.GetExisting(model.Id, model.Original, model.Short, model.RedirectsCount, model.UserId);
        }

        public async Task<Reference> FirstOrDefaultAsync(ReferenceQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var builder = new FilterDefinitionBuilder<Models.Reference>();
            var filter = builder.Empty;

            if (query.UserId != null)
            {
                filter = filter & builder.Eq(new StringFieldDefinition<Models.Reference, Guid>("userId"), query.UserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.ShortReference))
            {
                filter = filter & builder.Regex("short", new BsonRegularExpression(query.ShortReference));
            }

            var model = await this.references.Find(filter).FirstOrDefaultAsync();

            if (model == null)
            {
                return null;
            }

            return Reference.GetExisting(model.Id, model.Original, model.Short, model.RedirectsCount, model.UserId);
        }

        public async Task CreateAsync(Reference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var model = new Models.Reference
            {
                Id = reference.Id,
                Original = reference.Original,
                Short = reference.Short,
                RedirectsCount = reference.RedirectsCount,
                UserId = reference.OwnerId
            };

            await this.references.InsertOneAsync(model);
        }

        public async Task UpdateAsync(Reference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var model = new Models.Reference
            {
                Id = reference.Id,
                Original = reference.Original,
                Short = reference.Short,
                RedirectsCount = reference.RedirectsCount,
                UserId = reference.OwnerId
            };

            await this.references.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(reference.Id.ToString())), model);
        }

        public async Task DeleteAsync(Reference reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            await this.references.DeleteOneAsync(new BsonDocument("_id", new ObjectId(reference.Id.ToString())));
        }

        private void CreateIndexes()
        {
            var userIdIndex = new CreateIndexModel<Models.Reference>(
                Builders<Models.Reference>.IndexKeys.Ascending(x => x.UserId));

            this.references.Indexes.CreateOne(userIdIndex);

            var shortReferenceIndex = new CreateIndexModel<Models.Reference>(
                Builders<Models.Reference>.IndexKeys.Ascending(x => x.Short));

            this.references.Indexes.CreateOne(shortReferenceIndex);
        }
    }
}
