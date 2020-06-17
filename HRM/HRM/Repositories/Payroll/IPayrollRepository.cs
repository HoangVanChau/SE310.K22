using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using MongoDB.Driver;

namespace HRM.Repositories.Payroll
{
    public interface IPayrollRepository : IBaseRepository<Models.Cores.Payroll>
    {
        public Task<List<Models.Cores.Payroll>> QueryPayrolls(FilterDefinition<Models.Cores.Payroll> filters);
        public Task<Models.Cores.Payroll> QueryPayroll(FilterDefinition<Models.Cores.Payroll> filters);
    }
}