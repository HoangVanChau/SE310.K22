using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Models;
using HRM.Models.QueryParams;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.Contract
{
    public interface IContractRepository : IBaseRepository<Models.Cores.Contract>
    {
        Task<Models.Cores.Contract> GetByContractId(string id);
        Task<bool> UpdateByContractId(string id, UpdateDefinition<Models.Cores.Contract> updateDefinition);
        Task<Models.Cores.Contract> QueryContract(FilterDefinition<Models.Cores.Contract> filterDefinition);
        Task<List<Models.Cores.Contract>> QueryContracts(FilterDefinition<Models.Cores.Contract> filterDefinition, PagingParams pagingParams = null, ContractQuery query = null);
    }
}