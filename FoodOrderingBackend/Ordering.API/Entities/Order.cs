using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.API.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("totalPrice")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalPrice { get; set; }

        [BsonElement("createdOn")]
        public DateTime CreatedOn { get; set; }

        [BsonElement("completedOn")]
        public DateTime? CompletedOn { get; set; }

        [BsonElement("status")]
        [BsonIgnoreIfNull]
        public Status Status { get; set; }

        [BsonElement("paymentType")]
        [BsonIgnoreIfNull]
        public PaymentType PaymentType { get; set; }

        [BsonElement("paymentTypeId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PaymentTypeId { get; set; }

        [BsonElement("orderStatusId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StatusId { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }

        [BsonElement("user")]
        public User User { get; set; }

        [BsonElement("products")]
        public IEnumerable<Product> Products { get; set; }
    }
}
