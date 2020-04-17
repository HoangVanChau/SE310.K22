using System.Threading.Tasks;
using HRM.Repositories.Base;

namespace HRM.Repositories.Counter
{
    public interface ICounterRepository : IBaseRepository<Models.Cores.Counter>
    {
        public Task<uint> GetNextCounterValue(string sequenceName);
    }
}