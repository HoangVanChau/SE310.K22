using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models.QueryParams;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
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
            if (team.MembersId.Contains(userId))
            {
                team.MembersId.Remove(userId);
            }
            else
            {
                return false;
            }
            
            var updateDefine = Builders<Models.Cores.Team>
                .Update
                .Set(t => t.MembersId, team.MembersId)
                .Push(x => x.ModifyDate, DateTime.Now)
                .CurrentDate(x => x.LastModifyDate);
            
            var result = await Collection.UpdateOneAsync(x => x.TeamId == teamId, updateDefine);
            return result.ModifiedCount.Equals(1);
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

        public async Task<Models.Cores.Team> FindLeaderExistInAnyTeam(string userId)
        {
            return await Collection.Find(x => x.LeaderId == userId || x.MembersId.Contains(userId)).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteTeam(string teamId)
        {
            var result = await Collection.DeleteOneAsync(x => x.TeamId == teamId);
            return result.DeletedCount.Equals(1);
        }

        public async Task<List<Models.Cores.User>> GetMembersOfTeam(string teamId)
        {
            var team = await Collection.Aggregate()
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: t => t.MembersId,
                    foreignField: f => f.UserId,
                    @as: (Models.Cores.Team t) => t.Members
                )
                .FirstOrDefaultAsync();
            return team.Members;
        }

        public Task<List<Models.Cores.Team>> GetAllTeams(TeamQuery query)
        {
            var filterUser = query.UserId != null 
                ? Builders<Models.Cores.Team>.Filter.Or(
                    Builders<Models.Cores.Team>.Filter.Eq(x => x.LeaderId, query.UserId),
                    Builders<Models.Cores.Team>.Filter.Where(x => x.MembersId.Contains(query.UserId)
                    ))
                : FilterDefinition<Models.Cores.Team>.Empty;
            return Collection.Aggregate()
                .Match(filterUser)
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