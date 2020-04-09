using System;
using System.Threading.Tasks;
using HRM.Repositories.Base;

namespace HRM.Repositories.User
{
    public interface IUserRepository: IBaseRepository<Models.Cores.User>
    {
        Task<Models.Cores.User> FindUserByUserName(String userName); 
        Task<Models.Cores.User> FindUserByUserId(String userId); 
    }
}