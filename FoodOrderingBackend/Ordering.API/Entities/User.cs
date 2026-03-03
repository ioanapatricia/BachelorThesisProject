using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.API.Entities
{
    public class User
    {
        [BsonElement("externalId")]
        public int ExternalId { get; set; }

        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        public string LastName { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
