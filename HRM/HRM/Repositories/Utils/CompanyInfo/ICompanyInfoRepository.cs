using System.Threading.Tasks;
using HRM.Repositories.Base;

namespace HRM.Repositories.Utils.CompanyInfo
{
    public interface ICompanyInfoRepository : IBaseRepository<Models.Cores.CompanyInfo>
    {
        public Task<bool> UpdateCompanyInfo(Models.Cores.CompanyInfo updateDate);
        public Task<Models.Cores.CompanyInfo> GetCompanyInfo();
    }
}