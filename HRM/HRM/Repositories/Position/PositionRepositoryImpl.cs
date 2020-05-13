using System;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Position
{
    public class PositionRepositoryImpl : BaseRepositoryImpl<Models.Cores.Position>, IPositionRepository
    {
        public PositionRepositoryImpl(MongoDbService service) : base(service)
        {
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.PositionCollection;
        }

        public async Task<Models.Cores.Position> UpdateByPositionId(string id, UpdateDefinition<Models.Cores.Position> updateDefinition)
        {
            var finalDefine = updateDefinition.Push(x => x.ModifyDate, DateTime.Now);
            var result = await Collection.FindOneAndUpdateAsync(x => x.PositionId == id, finalDefine);
            return result;
        }

        public async Task<Models.Cores.Position> GetPositionById(string id)
        {
            var result = await Collection.Find(x => x.PositionId.Equals(id)).FirstOrDefaultAsync();
            return result;
        }
    }
}