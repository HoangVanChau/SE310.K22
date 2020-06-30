using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Extensions;
using HRM.Models.Cores;
using HRM.Models.QueryParams;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace HRM.Repositories.User
{
    public class UserRepositoryImpl: BaseRepositoryImpl<Models.Cores.User>, IUserRepository
    {
        private readonly IMongoCollection<Models.Cores.Team> _teamCollection;
        private readonly IMongoCollection<Models.Cores.Contract> _contractCollection;
        public UserRepositoryImpl(MongoDbService service) : base(service)
        {
            _teamCollection = service.GetDb().GetCollection<Models.Cores.Team>(Constants.Collections.TeamCollection);
            _contractCollection =
                service.GetDb().GetCollection<Models.Cores.Contract>(Constants.Collections.ContractCollection);
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

        public Task<List<Models.Cores.User>> Query(UserQuery query)
        {
            var filterQ = query?.Q != null
                ? Builders<Models.Cores.User>.Filter.Where(x => x.FullName.ToLower().Contains(query.Q.ToLower()))
                : FilterDefinition<Models.Cores.User>.Empty;
            var filterRole = query?.Role != null
                ? Builders<Models.Cores.User>.Filter.Eq(x => x.Role, query.Role)
                : FilterDefinition<Models.Cores.User>.Empty;

            var teamFilter = query?.Available switch
            {
                true => Builders<Models.Cores.User>.Filter.Where(x => x.Teams.Count == 0),
                false => Builders<Models.Cores.User>.Filter.Where(x => x.Teams.Count > 0),
                _ => Builders<Models.Cores.User>.Filter.Where(x => true)
            };
            
            var contractFilter = query?.Contractable switch
            {
                true => Builders<Models.Cores.User>.Filter.Where(x => x.Contracts.Count == 0),
                false => Builders<Models.Cores.User>.Filter.Where(x => x.Contracts.Count > 0),
                _ => Builders<Models.Cores.User>.Filter.Where(x => true)
            };

            if (query?.Role == Roles.Manager)
            {
                return Collection.Aggregate()
                    .Match(filterQ & filterRole)
                    .Lookup(
                        foreignCollection: _teamCollection,
                        foreignField: t => t.LeaderId,
                        localField: u => u.UserId,
                        @as: (Models.Cores.User u) => u.Teams
                    )
                    .Lookup(
                        foreignCollection: _contractCollection,
                        foreignField: t => t.UserId,
                        localField: u => u.UserId,
                        @as: (Models.Cores.User u) => u.Contracts
                    )
                    .Match(teamFilter)
                    .Match(contractFilter)
                    .ToListAsync();
            }

            return Collection.Aggregate()
                .Match(filterQ & filterRole)
                .Lookup(
                    foreignCollection: _teamCollection,
                    foreignField: t => t.MembersId,
                    localField: u => u.UserId,
                    @as: (Models.Cores.User u) => u.Teams
                    )
                .Lookup(
                    foreignCollection: _contractCollection,
                    foreignField: t => t.UserId,
                    localField: u => u.UserId,
                    @as: (Models.Cores.User u) => u.Contracts
                )
                .Match(teamFilter)
                .Match(contractFilter)
                .ToListAsync();
        }

        public async Task<bool> UpdateRemainDateOff(string userId, double change)
        {
            var updateDefine = Builders<Models.Cores.User>.Update.Inc(x => x.YearRemainDayOffs, change);
            var result = await Collection.UpdateOneAsync(x => x.UserId == userId, updateDefine);
            return result.ModifiedCount == 1;
        }
    }
}