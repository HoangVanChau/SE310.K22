using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Contract
{
    public class ContractRepositoryImpl : BaseRepositoryImpl<Models.Cores.Contract>, IContractRepository
    {
        public ContractRepositoryImpl(MongoDbService service) : base(service)
        {
            
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.ContractCollection;
        }

        public Task<Models.Cores.Contract> GetByContractId(string id)
        {
            return Collection.Find(x => x.ContractId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateByContractId(string id, UpdateDefinition<Models.Cores.Contract> updateDefinition)
        {
            var finalDefine = updateDefinition.Push(x => x.ModifyDate, DateTime.Now);
            var result = await Collection.UpdateOneAsync(x => x.ContractId == id, finalDefine);
            return result.ModifiedCount.Equals(1);
        }

        public Task<Models.Cores.Contract> QueryContract(FilterDefinition<Models.Cores.Contract> filterDefinition)
        {
            var result = Collection.Find(filterDefinition).FirstOrDefaultAsync();
            return result;
        }

        public Task<List<Models.Cores.Contract>> QueryContracts(FilterDefinition<Models.Cores.Contract> filterDefinition)
        {
            var result = Collection.Find(filterDefinition).ToListAsync();
            return result;
        }
    }
}