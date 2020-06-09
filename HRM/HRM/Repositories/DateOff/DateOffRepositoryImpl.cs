using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using HRM.Repositories.Team;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.DateOff
{
    public class DateOffRepositoryImpl : BaseRepositoryImpl<Models.Cores.DateOff>, IDateOffRepository
    {
        private readonly IMongoCollection<Models.Cores.Team> _teamCollection;
        private readonly IMongoCollection<Models.Cores.User> _userCollection;

        public DateOffRepositoryImpl(MongoDbService service) : base(service)
        {
            _teamCollection = service.GetDb().GetCollection<Models.Cores.Team>(Constants.Collections.TeamCollection);
            _userCollection = service.GetDb().GetCollection<Models.Cores.User>(Constants.Collections.UserCollection);
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.DateOffCollection;
        }

        public Task<List<Models.Cores.DateOff>> QueryDateOffs(FilterDefinition<Models.Cores.DateOff> filterDefinition)
        {
            var result = Collection.Aggregate()
                .Match(filterDefinition)
                .Lookup(
                    foreignCollection: _userCollection,
                    foreignField: u => u.UserId,
                    localField: d => d.UserId,
                    @as: (Models.Cores.DateOff o) => o.User
                ).ToListAsync();
            return result;
        }
        
        public Task<Models.Cores.DateOff> QueryDateOff(FilterDefinition<Models.Cores.DateOff> filterDefinition)
        {
            var result = Collection.Aggregate()
                .Match(filterDefinition)
                .Lookup(
                    foreignCollection: _userCollection,
                    foreignField: u => u.UserId,
                    localField: d => d.UserId,
                    @as: (Models.Cores.DateOff o) => o.User
                ).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Models.Cores.DateOff> QueryDateOff(string dateOffId)
        {
            var result = await Collection.Aggregate()
                .Match(x => x.Id == dateOffId)
                .FirstOrDefaultAsync();
            return result;
        }
    }
}