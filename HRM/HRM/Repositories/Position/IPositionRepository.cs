using System.Threading.Tasks;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.Position
{
    public interface IPositionRepository : IBaseRepository<Models.Cores.Position>
    {
        Task<Models.Cores.Position> UpdateByPositionId(string id, UpdateDefinition<Models.Cores.Position> updateDefinition);
        Task<Models.Cores.Position> GetPositionById(string id);
    }
}