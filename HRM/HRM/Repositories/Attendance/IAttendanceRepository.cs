using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Constants;
using HRM.Models;
using HRM.Models.QueryParams;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.Attendance
{
    public interface IAttendanceRepository : IBaseRepository<Models.Cores.Attendance>
    {
        public Task<List<Models.Cores.Attendance>> InsertManyAttendance(List<Models.Cores.Attendance> listImport);
        public Task<List<Models.Cores.Attendance>> QueryAttendance(FilterDefinition<Models.Cores.Attendance> customFilter = null, PagingParams pagingParams = null);
    }
}