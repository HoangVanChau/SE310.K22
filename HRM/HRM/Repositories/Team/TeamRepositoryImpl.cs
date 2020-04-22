using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Team
{
    public class TeamRepositoryImpl: BaseRepositoryImpl<Models.Cores.Team>, ITeamRepository
    {

        private readonly IMongoCollection<Models.Cores.User> _userCollection;
        
        public TeamRepositoryImpl(MongoDbService service) : base(service)
        {
            _userCollection = service.GetDb().GetCollection<Models.Cores.User>(Constants.Collections.UserCollection);
        }
        
        public override string GetCollectionName()
        {
            return Constants.Collections.TeamCollection;
        }

        public Task<Models.Cores.Team> GetTeamByTeamId(string teamId)
        {
            return Collection.Find(x => x.TeamId == teamId).FirstOrDefaultAsync();
        }

        public Task<bool> AddNewUserToTeam(string teamId, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteUserFromTeam(string teamId, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Models.Cores.Team>> GetAllTeams()
        {
            return Collection.Aggregate()
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: l => l.LeaderId,
                    foreignField: f => f.UserId,
                    @as: (Models.Cores.Team t) => t.Leaders
                    )
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: l => l.MembersId,
                    foreignField: f => f.UserId,
                    @as: (Models.Cores.Team t) => t.Members
                    )
                .ToListAsync();
        }
    }
}