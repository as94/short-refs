namespace ShortRefs.Data.Mongo.Registries
{
    using Microsoft.Extensions.DependencyInjection;

    using MongoDB.Driver;

    using ShortRefs.Data.Mongo.Repositories;
    using ShortRefs.Domain.Repositories;

    public static class CounterRegistry
    {
        public static void RegisterCounters(this IServiceCollection services)
        {
            services.AddSingleton<ISequenceCounterRepository>(x => new MongoSequenceCounterRepository(x.GetService<IMongoClient>()));
        }
    }
}
