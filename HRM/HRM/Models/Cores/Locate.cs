using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    public class Locate
    {    
        [BsonElement("id")]
        public int LocateId { get; set; }
        [BsonElement("name")]
        public string LocateName { get; set; }
    }
}