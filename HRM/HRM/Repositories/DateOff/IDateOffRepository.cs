using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.DateOff
{
    public interface IDateOffRepository : IBaseRepository<Models.Cores.DateOff>
    {
        public Task<List<Models.Cores.DateOff>> QueryDateOffs(FilterDefinition<Models.Cores.DateOff> filterDefinition);
        public Task<Models.Cores.DateOff> QueryDateOff(FilterDefinition<Models.Cores.DateOff> filterDefinition);
    }
}