#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class Contract : BaseModel
    {
        public string ContractId { get; set; }
        public string? ContractName { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string TeamId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public string PositionId { get; set; }
        public double? MonthlyNetSalary { get; set; }
        public double? HourlyNetSalary { get; set; }
        public bool? OfficialEmployee { get; set; }
        public ExtraBonus? ExtraBonus { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EndDate { get; set; }
        public DateTime? DisableDate { get; set; }
        public int BaseDateOff { get; set; } = 12;
        public int ExtraDateOff { get; set; }

        public List<User>? User { get; set; }
        public List<Position>? Position { get; set; }
        public List<Team>? Team { get; set; }
    }
}