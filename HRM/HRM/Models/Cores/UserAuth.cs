using System;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class UserAuth : User
    {
        public String HashPassword { get; set; }
    }
}