using System;
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

        public async Task<Models.Cores.Contract> UpdateByContractId(string id, UpdateDefinition<Models.Cores.Contract> updateDefinition)
        {
            var finalDefine = updateDefinition.Push(x => x.ModifyDate, DateTime.Now);
            var result = await Collection.FindOneAndUpdateAsync(x => x.PositionId == id, finalDefine);
            return result;
        }
    }
}