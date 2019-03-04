namespace ShortRefs.Data.Mongo.Models
{
    using MongoDB.Bson.Serialization.Attributes;

    internal sealed class Counter
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("count")]
        public long Count { get; set; }
    }
}
