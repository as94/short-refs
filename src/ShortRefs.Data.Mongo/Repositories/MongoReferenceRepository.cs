using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ShortRefs.Data.Mongo.Tests")]

namespace ShortRefs.Data.Mongo.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using ShortRefs.Domain.Models.References;
    using ShortRefs.Domain.Repositories;

    internal sealed class MongoReferenceRepository : IReferenceRepository
    {
        private readonly IMongoCollection<Models.Reference> references;

        public MongoReferenceRepository(IMongoClient mongoClient)
        {
            if (mongoClient == null)
            {
                throw new ArgumentNullException(nameof(mongoClient));
            }

            var database = mongoClient.GetDatabase("ReferenceDb");
            this.references = database.GetCollection<Models.Reference>("references");

            this.CreateIndexes();
        }

        public async Task<Reference> GetAsync(long id, CancellationToken cancellationToken)
        {
            var model = await this.references.Find(r => r.Id == id)
                            .FirstOrDefaultAsync(cancellationToken);

            if (model == null)
            {
                return null;
            }

            return Reference.GetExisting(model.Id, model.Original, model.Short, model.RedirectsCount, model.UserId);
        }

        public async Task<Reference> FirstOrDefaultAsync(ReferenceQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var filter = GetFilter(query);
            var model = await this.references.Find(filter)
                            .FirstOrDefaultAsync(cancellationToken);

            if (model == null)
            {
                return null;
            }

            return Reference.GetExisting(model.Id, model.Original, model.Short, model.RedirectsCount, model.UserId);
        }

        public async Task<IReadOnlyCollection<Reference>> FindAsync(ReferenceQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var filter = GetFilter(query);
            var models = await this.references.Find(filter).ToListAsync(cancellationToken);

            return models
                .Select(model => Reference.GetExisting(model.Id, model.Original, model.Short, model.RedirectsCount, model.UserId))
                .ToList()
                .AsReadOnly();
        }

        public async Task CreateAsync(Reference reference, CancellationToken cancellationToken)
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

            await this.references.InsertOneAsync(model, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(Reference reference, CancellationToken cancellationToken)
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

            await this.references.ReplaceOneAsync(r => r.Id == model.Id, model, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(Reference reference, CancellationToken cancellationToken)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            await this.references.DeleteOneAsync(r => r.Id == reference.Id, cancellationToken);
        }

        public async Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return await this.references.CountDocumentsAsync(new BsonDocument(), cancellationToken: cancellationToken);
        }

        private static FilterDefinition<Models.Reference> GetFilter(ReferenceQuery query)
        {
            var builder = new FilterDefinitionBuilder<Models.Reference>();
            var filter = builder.Empty;

            if (query.UserId != null)
            {
                filter = filter & builder.Eq(new StringFieldDefinition<Models.Reference, Guid>("userId"), query.UserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.OriginalReference))
            {
                filter = filter & builder.Regex("original", new BsonRegularExpression(query.OriginalReference));
            }

            return filter;
        }

        private void CreateIndexes()
        {
            var originalReferenceIndex = new CreateIndexModel<Models.Reference>(
                Builders<Models.Reference>.IndexKeys.Ascending(x => x.Original),
                new CreateIndexOptions { Unique = true });

            this.references.Indexes.CreateOne(originalReferenceIndex);
        }
    }
}
