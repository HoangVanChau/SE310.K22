using System;
using HRM.Models.Bases;

namespace HRM.Models.Cores
{
    public class Holiday : BaseModel
    {
        public DateTime Date { get; set; }
        public String Description { get; set; }
    }
}