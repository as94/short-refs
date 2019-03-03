namespace ShortRefs.Data.Mongo.Registries
{
    using Microsoft.Extensions.DependencyInjection;

    using MongoDB.Driver;

    using ShortRefs.Data.Mongo.Repositories;
    using ShortRefs.Domain.Repositories;

    public static class ReferenceRegistry
    {
        public static void RegisterReferences(this IServiceCollection services)
        {
            services.AddSingleton<IReferenceRepository>(x => new MongoReferenceRepository(x.GetService<IMongoClient>()));
        }
    }
}
