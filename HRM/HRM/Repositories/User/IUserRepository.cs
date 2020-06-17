using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.User
{
    public interface IUserRepository: IBaseRepository<Models.Cores.User>
    {
        Task<List<HRM.Models.Cores.User>> GetUsersByRole(string role);
        Task<Models.Cores.User> FindUserByUserName(String userName); 
        Task<Models.Cores.User> FindUserByUserId(String userId);
        Task<bool> UpdateUserByUserId(String userId, UpdateDefinition<Models.Cores.User> updateDefinition);
        Task<bool> DeleteUserByUserid(String userId);
        Task<List<Models.Cores.User>> Query(String q, string available, string role);
        Task<bool> UpdateRemainDateOff(String userId, double change);
    }
}