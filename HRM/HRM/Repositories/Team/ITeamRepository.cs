using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.Team
{
    public interface ITeamRepository: IBaseRepository<Models.Cores.Team>
    {
        public Task<Models.Cores.Team> GetTeamByTeamId(string teamId);
        public Task<bool> AddNewUserToTeam(string teamId, string userId);
        public Task<bool> DeleteUserFromTeam(string teamId, string userId);
        public List<Models.Cores.Team> FindUserExistInAnyTeam(string userId);
        public Task<List<Models.Cores.Team>> GetAllTeams();
        public Task<bool> UpdateTeamInfoByTeamId(string teamId, UpdateDefinition<Models.Cores.Team> updateDefine);
        public Task<Models.Cores.Team> FindLeaderExistInAnyTeam(string userId);
    }
}