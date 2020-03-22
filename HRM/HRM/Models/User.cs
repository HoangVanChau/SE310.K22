using System;
using HRM.Models.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models
{
    public class User : BaseModel
    {
        public String FullName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
    }
}