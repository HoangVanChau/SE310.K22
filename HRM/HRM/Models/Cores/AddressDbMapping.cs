using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class Province
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId BsonId { get; set; }
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("code")]
        public string Code { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("districts")]
        public List<District> Districts { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class District
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId BsonId { get; set; }
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("code")]
        public string Code { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("wards")]
        public List<Ward> Wards { get; set; }
    }
    
    [BsonIgnoreExtraElements]
    public class Ward
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public ObjectId BsonId { get; set; }
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("code")]
        public string Code { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
    }
}