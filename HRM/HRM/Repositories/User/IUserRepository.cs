using System;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.User
{
    public interface IUserRepository: IBaseRepository<Models.Cores.User>
    {
        Task<Models.Cores.User> FindUserByUserName(String userName); 
        Task<Models.Cores.User> FindUserByUserId(String userId);
        Task<bool> UpdateUserByUserId(String userId, UpdateDefinition<Models.Cores.User> updateDefinition);
    }
}