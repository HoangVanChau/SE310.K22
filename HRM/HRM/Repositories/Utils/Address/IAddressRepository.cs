using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Models.Cores;

namespace HRM.Repositories.Utils.Address
{
    public interface IAddressRepository
    {
        public Task<List<Locate>> GetProvinces();
        public Task<List<Locate>> GetDistricts(string provinceId);
        public Task<List<Locate>> GetWards(string districtId);
    }
}