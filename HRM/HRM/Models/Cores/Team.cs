using System.Collections.Generic;
using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class Team : BaseModel
    {
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public List<string> MembersId { get; set; }
        public List<User> Members { get; set; }
        public List<User> Leaders { get; set; }
        public string LeaderId { get; set; }
        public string TeamAvatarImageId { get; set; }
    }
}