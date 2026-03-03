using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.API.Entities
{
    public class Status
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
