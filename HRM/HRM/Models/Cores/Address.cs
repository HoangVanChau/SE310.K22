using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class Address
    {
        public Locate Province { get; set; }
        public Locate District { get; set; }
        public Locate Ward { get; set; }
        public string DetailAddress { get; set; }
    }
}