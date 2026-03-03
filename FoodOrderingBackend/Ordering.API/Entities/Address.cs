using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.API.Entities
{
    public class Address
    {
        [BsonElement("city")]
        public string City { get; set; }
        [BsonElement("county")]
        public string County { get; set; }
        [BsonElement("sector")]
        public int? Sector { get; set; }
        [BsonElement("street")]
        public string Street { get; set; }
        [BsonElement("streetNumber")]
        public int StreetNumber { get; set; }
        [BsonElement("building")]
        public string Building { get; set; }
        [BsonElement("floor")]
        public int? Floor { get; set; }
        [BsonElement("entrance")]
        public string Entrance { get; set; }
        [BsonElement("apartmentNumber")]
        public int? ApartmentNumber { get; set; }
    }
}
