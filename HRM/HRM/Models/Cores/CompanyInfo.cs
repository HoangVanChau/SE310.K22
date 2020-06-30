using System;
using HRM.Models.Bases;
using MongoDB.Bson.Serialization.Attributes;

namespace HRM.Models.Cores
{
    [BsonIgnoreExtraElements]
    public class CompanyInfo : BaseModel
    {
        public String CompanyName { get; set; }
        public String Avatar { get; set; }
        public String TaxNumber { get; set; }
        public String CompanyType { get; set; }
        public Address Address { get; set; }
        public String LegalRepresentative { get; set; }
        public DateTime LicenseDate { get; set; }
        public DateTime ActiveDate { get; set; }
        public String PhoneNumber { get; set; }
        public String Status { get; set; }
        public String RawLicenseDate { get; set; }
        public String RawActiveDate { get; set; }
    }
}