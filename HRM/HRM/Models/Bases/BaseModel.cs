using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Bases
{    
    [BsonIgnoreExtraElements]
    public abstract class BaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [JsonIgnore]
        public DateTime LastModifyDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [JsonIgnore]
        public List<DateTime> ModifyDate { get; set; } 
    }
}