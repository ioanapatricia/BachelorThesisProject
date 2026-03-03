using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.API.Entities
{
    public class Product
    {
        [BsonElement("externalId")]
        public int ExternalId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("categoryName")]
        public string CategoryName { get; set; }

        [BsonElement("salePercentage")] //TODO be nullable
        public int? SalePercentage { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        [BsonElement("weight")]
        public float Weight { get; set; }
    }
}
