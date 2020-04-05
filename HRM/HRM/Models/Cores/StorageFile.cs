using System;
using HRM.Models.Base;

namespace HRM.Models.Cores
{
    public abstract class StorageFile: BaseModel
    {
        public String RawBinary { get; set; }
    }
}