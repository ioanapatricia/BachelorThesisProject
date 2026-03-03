using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SiteManagement.API.Entities
{
    public class SiteSettings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("schedule")]
        public string Schedule { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("fax")]
        public string Fax { get; set; }

        [BsonElement("feedbackEmail")]
        public string FeedbackEmail { get; set; }

        [BsonElement("businessEmail")]
        public string BusinessEmail { get; set; }

        [BsonElement("facebookUrl")]
        public string FacebookUrl { get; set; }

        [BsonElement("instagramUrl")]
        public string InstagramUrl { get; set; }
    }
}
