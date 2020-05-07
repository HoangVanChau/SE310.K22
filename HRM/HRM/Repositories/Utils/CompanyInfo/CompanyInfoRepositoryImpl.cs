using System;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Utils.CompanyInfo
{
    public class CompanyInfoRepositoryImpl : BaseRepositoryImpl<Models.Cores.CompanyInfo>, ICompanyInfoRepository
    {
        public CompanyInfoRepositoryImpl(MongoDbService service) : base(service)
        {
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.CompanyInfoCollection;
        }

        public async Task<bool> UpdateCompanyInfo(Models.Cores.CompanyInfo updateData)
        {
            var updateDefine = Builders<Models.Cores.CompanyInfo>
                .Update
                .Set(x => x.LastModifyDate, DateTime.Now);

            if (updateData.CompanyName != null)
            {
                updateDefine = updateDefine.Set(x => x.CompanyName, updateData.CompanyName);
            }
            if (updateData.Address != null)
            {
                updateDefine = updateDefine.Set(x => x.Address, updateData.Address);
            }
            if (updateData.Avatar != null)
            {
                updateDefine = updateDefine.Set(x => x.Avatar, updateData.Avatar);
            }
            if (updateData.Status != null)
            {
                updateDefine = updateDefine.Set(x => x.Status, updateData.Status);
            }
            if (updateData.CompanyType != null)
            {
                updateDefine = updateDefine.Set(x => x.CompanyType, updateData.CompanyType);
            }
            if (updateData.LegalRepresentative != null)
            {
                updateDefine = updateDefine.Set(x => x.LegalRepresentative, updateData.LegalRepresentative);
            }
            if (updateData.PhoneNumber != null)
            {
                updateDefine = updateDefine.Set(x => x.PhoneNumber, updateData.PhoneNumber);
            }
            if (updateData.TaxNumber != null)
            {
                updateDefine = updateDefine.Set(x => x.TaxNumber, updateData.TaxNumber);
            }
            if (updateData.RawActiveDate != null)
            {
                var newActiveDate = DateTime.Parse(updateData.RawActiveDate);
                updateDefine = updateDefine.Set(x => x.ActiveDate, newActiveDate);
                updateDefine = updateDefine.Set(x => x.RawActiveDate, updateData.RawActiveDate);
            }
            if (updateData.RawLicenseDate != null)
            {
                var newLicenseDate = DateTime.Parse(updateData.RawLicenseDate);
                updateDefine = updateDefine.Set(x => x.LicenseDate, newLicenseDate);
                updateDefine = updateDefine.Set(x => x.RawLicenseDate, updateData.RawLicenseDate);
            }
            
            var result = await Collection.UpdateOneAsync(x => true, updateDefine);
            return result.MatchedCount.Equals(1);
        }

        public Task<Models.Cores.CompanyInfo> GetCompanyInfo()
        {
            return Collection.Find(x => true).FirstOrDefaultAsync();
        }
    }
}