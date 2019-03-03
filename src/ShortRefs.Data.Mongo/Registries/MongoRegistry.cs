namespace ShortRefs.Data.Mongo.Registries
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using MongoDB.Driver;

    public static class MongoRegistry
    {
        public static void RegisterMongo(this IServiceCollection services, string mongoConnectionString)
        {
            if (mongoConnectionString == null)
            {
                throw new ArgumentNullException(nameof(mongoConnectionString));
            }

            services.AddSingleton<IMongoClient>(x => new MongoClient(mongoConnectionString));
        }
    }
}
