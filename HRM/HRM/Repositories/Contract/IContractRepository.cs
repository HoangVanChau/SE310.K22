using System.Threading.Tasks;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.Contract
{
    public interface IContractRepository : IBaseRepository<Models.Cores.Contract>
    {
        Task<Models.Cores.Contract> GetByContractId(string id);
        Task<Models.Cores.Contract> UpdateByContractId(string id, UpdateDefinition<Models.Cores.Contract> updateDefinition);
    }
}