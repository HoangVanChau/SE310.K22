using HRM.Models.Bases;

namespace HRM.Models.Cores
{
    public class Notification : BaseModel
    {
        public string Type { get; set; }
        public string Data { get; set; }
        public string TargetUserId { get; set; }
    }
}