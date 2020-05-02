using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Repositories.Base;

namespace HRM.Repositories.AuthRepository
{
    public interface IAuthRepository: IBaseRepository<UserAuth>
    {
        public Task<UserAuth> FindUserAuthByUserName(string userName);
        public Task<UserAuth> FindUserAuthByUserId(string userId);
        public Task<bool> UpdateUserPassword(string userId, string newHashPassword);
    }
}