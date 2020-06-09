using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class DateOff : BaseModel
    {
        [Required]
        public TimeSpan StartOff { get; set; }
        [Required]
        public TimeSpan EndOff { get; set; }
        [Required]
        [BsonDateTimeOptions(DateOnly = true)]
        public DateTime Date { get; set; }
        [Required]
        public String Reason { get; set; }
        public Status Status { get; set; }
        [BsonIgnore] public bool IsApprove { get; set; }
        public String RejectReason { get; set; }
        public string UserId { get; set; } 
        public string TeamId { get; set; } 
        
        public List<User> User { get; set; }
    }
}