using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            return "User";
        }

        public Models.Cores.User FindUserByUserName(string userName)
        {
            return _collection.AsQueryable().Where(u => u.UserName == userName).FirstOrDefault();
        }

        public Models.Cores.User FindUserByUserId(string userId)
        {
            return _collection.AsQueryable().Where(u => u.UserId == userId).FirstOrDefault();
        }
    }
}