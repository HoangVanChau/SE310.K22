using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Extensions;
using HRM.Models.Cores;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace HRM.Repositories.User
{
    public class UserRepositoryImpl: BaseRepositoryImpl<Models.Cores.User>, IUserRepository
    {
        private readonly IMongoCollection<Models.Cores.Team> _teamCollection;
        public UserRepositoryImpl(MongoDbService service) : base(service)
        {
            _teamCollection = service.GetDb().GetCollection<Models.Cores.Team>(Constants.Collections.TeamCollection);
        }

        public override string GetCollectionName()
        {
            return Collections.UserCollection;
        }

        public async Task<Models.Cores.User> FindUserByUserName(string userName)
        {
            return await Collection.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<Models.Cores.User> FindUserByUserId(string userId)
        {
            var user = await Collection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UpdateUserByUserId(string userId, UpdateDefinition<Models.Cores.User> updateDefinition)
        {
            var finalUpdate = updateDefinition.Push(e => e.ModifyDate, DateTime.Now);
            var result = await Collection.UpdateOneAsync(x => x.UserId == userId, finalUpdate);
            return result.ModifiedCount.Equals(1);
        }

        public async Task<List<Models.Cores.User>> GetUsersByRole(string role)
        {
            return await Collection.Find(x => x.Role == role).ToListAsync();
        }

        public async Task<bool> DeleteUserByUserid(string userId)
        {
            var result = await Collection.DeleteOneAsync(x => x.UserId == userId);
            return result.DeletedCount == 1;
        }    

        public Task<List<Models.Cores.User>> Query(string q, string available, string role)
        {
            var userFilter = Builders<Models.Cores.User>.Filter.Where(x =>
                (role == null || x.Role.Equals(role)) &&
                (q == null || x.FullName.ToLower().Contains((q ?? "").ToLower())));

            var teamFilter = available switch
            {
                "true" => Builders<Models.Cores.User>.Filter.Where(x => x.Teams.Count == 0),
                "false" => Builders<Models.Cores.User>.Filter.Where(x => x.Teams.Count > 0),
                _ => Builders<Models.Cores.User>.Filter.Where(x => true)
            };

            return Collection.Aggregate()
                .Match(userFilter)
                .Lookup(
                    foreignCollection: _teamCollection,
                    foreignField: t => t.MembersId,
                    localField: u => u.UserId,
                    @as: (Models.Cores.User u) => u.Teams
                    )
                .Match(teamFilter)
                .ToListAsync();
        }
    }
}