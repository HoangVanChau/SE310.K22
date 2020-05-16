using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Models;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Attendance
{
    public class AttendanceRepoImpl : BaseRepositoryImpl<Models.Cores.Attendance>, IAttendanceRepository
    {
        public AttendanceRepoImpl(MongoDbService service) : base(service)
        {
            
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.AttendanceCollection;
        }

        public async Task<List<Models.Cores.Attendance>> InsertManyAttendance(List<Models.Cores.Attendance> listImport)
        {
            await Collection.InsertManyAsync(listImport);
            return listImport;
        }

        public Task<List<Models.Cores.Attendance>> QueryAttendance(FilterDefinition<Models.Cores.Attendance> customFilter = null, PagingParams pagingParams = null)
        {
            var query = Collection.Find(customFilter);
            if (pagingParams?.Page != null)
            {
                query.Limit(Constants.Paging.PageLimit).Skip((pagingParams.Page - 1) * Constants.Paging.PageLimit);
            }
            return query.ToListAsync();
        }
    }
}