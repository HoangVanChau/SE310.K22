using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Models;
using HRM.Models.QueryParams;
using HRM.Repositories.Base;
using HRM.Repositories.Position;
using HRM.Repositories.Team;
using HRM.Repositories.User;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Contract
{
    public class ContractRepositoryImpl : BaseRepositoryImpl<Models.Cores.Contract>, IContractRepository
    {
        private readonly IMongoCollection<Models.Cores.Team> _teamCollection;
        private readonly IMongoCollection<Models.Cores.User> _userCollection;
        private readonly IMongoCollection<Models.Cores.Position> _positionCollection;
        public ContractRepositoryImpl(MongoDbService service) : base(service)
        {
            _teamCollection = service.GetDb().GetCollection<Models.Cores.Team>(Constants.Collections.TeamCollection);
            _userCollection = service.GetDb().GetCollection<Models.Cores.User>(Constants.Collections.UserCollection);
            _positionCollection = service.GetDb().GetCollection<Models.Cores.Position>(Constants.Collections.PositionCollection);
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.ContractCollection;
        }

        public Task<Models.Cores.Contract> GetByContractId(string id)
        {
            var result = Collection.Aggregate()
                .Match(x => x.ContractId.Equals(id))
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: c => c.UserId,
                    foreignField: u => u.UserId,
                    @as: (Models.Cores.Contract c) => c.User
                ).Lookup(
                    foreignCollection: _teamCollection,
                    localField: c => c.TeamId,
                    foreignField: u => u.TeamId,
                    @as: (Models.Cores.Contract c) => c.Team
                ).Lookup(
                    foreignCollection: _positionCollection,
                    localField: c => c.PositionId,
                    foreignField: u => u.PositionId,
                    @as: (Models.Cores.Contract c) => c.Position
                )
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> UpdateByContractId(string id, UpdateDefinition<Models.Cores.Contract> updateDefinition)
        {
            var finalDefine = updateDefinition.Push(x => x.ModifyDate, DateTime.Now);
            var result = await Collection.UpdateOneAsync(x => x.ContractId == id, finalDefine);
            return result.ModifiedCount.Equals(1);
        }

        public Task<Models.Cores.Contract> QueryContract(FilterDefinition<Models.Cores.Contract> filterDefinition)
        {
            var result = Collection.Aggregate()
                .Match(filterDefinition)
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: c => c.UserId,
                    foreignField: u => u.UserId,
                    @as: (Models.Cores.Contract c) => c.User
                ).Lookup(
                    foreignCollection: _teamCollection,
                    localField: c => c.TeamId,
                    foreignField: u => u.TeamId,
                    @as: (Models.Cores.Contract c) => c.Team
                ).Lookup(
                    foreignCollection: _positionCollection,
                    localField: c => c.PositionId,
                    foreignField: u => u.PositionId,
                    @as: (Models.Cores.Contract c) => c.Position
                ).FirstOrDefaultAsync();
            return result;
        }

        public Task<List<Models.Cores.Contract>> QueryContracts(FilterDefinition<Models.Cores.Contract> filterDefinition, PagingParams pagingParams = null, ContractQuery query = null)
        {
            var filterUser = query?.UserId != null 
                ? Builders<Models.Cores.Contract>.Filter.Eq(x => x.UserId, query.UserId)
                : FilterDefinition<Models.Cores.Contract>.Empty;
            
            var filterActive = query?.Active != null 
                ? Builders<Models.Cores.Contract>.Filter.Eq(x => x.Active, query.Active)
                : FilterDefinition<Models.Cores.Contract>.Empty;
            
            var pineLine = Collection.Aggregate()
                .Match(filterDefinition & filterUser & filterActive)
                .Lookup(
                    foreignCollection: _userCollection,
                    localField: c => c.UserId,
                    foreignField: u => u.UserId,
                    @as: (Models.Cores.Contract c) => c.User
                ).Lookup(
                    foreignCollection: _teamCollection,
                    localField: c => c.TeamId,
                    foreignField: u => u.TeamId,
                    @as: (Models.Cores.Contract c) => c.Team
                ).Lookup(
                    foreignCollection: _positionCollection,
                    localField: c => c.PositionId,
                    foreignField: u => u.PositionId,
                    @as: (Models.Cores.Contract c) => c.Position
                );
            
            if (pagingParams?.Page != null)
            {
                pineLine = pineLine.Skip((pagingParams.Page - 1) * Constants.Paging.PageLimit ?? 0).Limit(Constants.Paging.PageLimit);
            }

            var result = pineLine.ToListAsync();
            return result;
        }
    }
}