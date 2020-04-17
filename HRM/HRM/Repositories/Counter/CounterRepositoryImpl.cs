using System.Threading.Tasks;
using HRM.Constants;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Counter
{
    public class CounterRepositoryImpl: BaseRepositoryImpl<Models.Cores.Counter>, ICounterRepository
    {
        public CounterRepositoryImpl(MongoDbService service) : base(service)
        {
            
        }

        public override string GetCollectionName()
        {
            return Collections.CounterCollection;
        }

        public async Task<uint> GetNextCounterValue(string sequenceName)
        {
            var filter = Builders<Models.Cores.Counter>.Filter.Eq(a => a.Name, sequenceName);
            var update = Builders<Models.Cores.Counter>.Update.Inc<uint>(a => a.Value, 1);
            var sequence = await _collection.FindOneAndUpdateAsync(filter, update);

            return sequence.Value;
        }
    }
}