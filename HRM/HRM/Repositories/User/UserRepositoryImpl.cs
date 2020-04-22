using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Extensions;
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
            return await Collection.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<Models.Cores.User> FindUserByUserId(string userId)
        {
            var user = await Collection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            return user.WithoutPassword();
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
    }
}