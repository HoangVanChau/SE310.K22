using HRM.Models.Base;

namespace HRM.Models.Cores
{
    public class Counter: BaseModel
    {
        public string Name { get; set; }
        public uint Value { get; set; }
    }
}