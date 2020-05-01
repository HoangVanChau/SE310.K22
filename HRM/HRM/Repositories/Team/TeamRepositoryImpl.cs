using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HRM.Repositories.Team
{
    public class TeamRepositoryImpl: BaseRepositoryImpl<Models.Cores.Team>, ITeamRepository
    {
        //foreign collection for Lookup
        private readonly IMongoCollection<Models.Cores.User> _userCollection;
        
        public TeamRepositoryImpl(MongoDbService service) : base(service)
        {
            _userCollection = service.GetDb().GetCollection<Models.Cores.User>(Constants.Collections.UserCollection);
        }
        
        public override string GetCollectionName()
        {
            return Constants.Collections.TeamCollection;
        }

        public async Task<Models.Cores.Team> GetTeamByTeamId(string teamId)
        {
            return await Collection.Aggregate()
                .Match(x => x.TeamId == teamId)
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: x => x.LeaderId,
                    foreignField: u => u.UserId,
                    @as: (Models.Cores.Team t) => t.Leaders
                )
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: x => x.MembersId,
                    foreignField: u => u.UserId,
                    @as: (Models.Cores.Team t) => t.Members
                    )
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddNewUserToTeam(string teamId, string userId)
        {
            var updateDefine = Builders<Models.Cores.Team>
                .Update
                .Push(t => t.MembersId, userId);
            var result = await Collection.UpdateOneAsync(t => t.TeamId == teamId, updateDefine);
            return result.ModifiedCount.Equals(1);
        }

        public async Task<bool> DeleteUserFromTeam(string teamId, string userId)
        {
            var team = await Collection.Find(x => x.TeamId == teamId).FirstOrDefaultAsync();
            team.MembersId.Remove(userId);
            
            var updateDefine = Builders<Models.Cores.Team>
                .Update
                .Set(t => t.MembersId, team.MembersId)
                .Push(x => x.ModifyDate, DateTime.Now)
                .CurrentDate(x => x.LastModifyDate);
            
            var result = await Collection.FindOneAndUpdateAsync(x => x.TeamId == teamId, updateDefine);
            return true;
        }

        public List<Models.Cores.Team> FindUserExistInAnyTeam(string userId)
        {
            var result = Collection
                .AsQueryable()
                .Where(x => x.MembersId.Contains(userId))
                .ToList();
            return result;
        }

        public async Task<bool> UpdateTeamInfoByTeamId(string teamId, UpdateDefinition<Models.Cores.Team> updateDefine)
        {
            var finalDefine = updateDefine.Push(x => x.ModifyDate, DateTime.Now);
            var result = await Collection.UpdateOneAsync(x => x.TeamId == teamId, finalDefine);
            return result.IsAcknowledged && result.ModifiedCount.Equals(1);
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