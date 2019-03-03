namespace ShortRefs.Data.Mongo.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    internal sealed class Reference
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("original")]
        public string Original { get; set; }

        [BsonElement("short")]
        public string Short { get; set; }

        [BsonElement("redirectsCount")]
        public int RedirectsCount { get; set; }
    }
}
