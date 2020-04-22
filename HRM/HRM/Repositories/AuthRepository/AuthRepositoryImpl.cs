using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.AuthRepository
{
    public class AuthRepositoryImpl: BaseRepositoryImpl<UserAuth>, IAuthRepository
    {
        public AuthRepositoryImpl(MongoDbService service) : base(service)
        {
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.UserCollection;
        }

        public async Task<UserAuth> FindUserAuthByUserName(string userName)
        {
            return await Collection.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<UserAuth> FindUserAuthByUserId(string userId)
        {
            return await Collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
        }
    }
}