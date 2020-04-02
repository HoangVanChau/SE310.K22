using System;
using System.Threading.Tasks;
using HRM.Repositories.Base;

namespace HRM.Repositories.User
{
    public interface IUserRepository: IBaseRepository<Models.Cores.User>
    {
        Models.Cores.User FindUserByUserName(String userName); 
        Models.Cores.User FindUserByUserId(String userId); 
    }
}