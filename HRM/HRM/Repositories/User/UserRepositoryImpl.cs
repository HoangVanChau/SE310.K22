using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace HRM.Repositories.User
{
    public class UserRepositoryImpl: BaseRepositoryImpl<Models.Cores.User>, IUserRepository
    {
        public UserRepositoryImpl(MongoDbService service) : base(service)
        {
            
        }

        public override string GetCollectionName()
        {
            return Collections.UserCollection;
        }

        public async Task<Models.Cores.User> FindUserByUserName(string userName)
        {
            return await _collection.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<Models.Cores.User> FindUserByUserId(string userId)
        {
            return await _collection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUserByUserId(string userId, UpdateDefinition<Models.Cores.User> updateDefinition)
        {
            var finalUpdate = updateDefinition.Push(e => e.ModifyDate, DateTime.Now);
            var result = await _collection.UpdateOneAsync(x => x.UserId == userId, finalUpdate);
            return result.ModifiedCount.Equals(1);
        }
    }
}