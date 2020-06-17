using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;
using MongoDB.Driver;

namespace HRM.Repositories.Payroll
{
    public class PayrollRepositoryImpl : BaseRepositoryImpl<Models.Cores.Payroll>, IPayrollRepository
    {
        public PayrollRepositoryImpl(MongoDbService service) : base(service)
        {
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.PayrollCollection;
        }

        public Task<List<Models.Cores.Payroll>> QueryPayrolls(FilterDefinition<Models.Cores.Payroll> filters)
        {
            return Collection.Aggregate().Match(filters).ToListAsync();
        }

        public Task<Models.Cores.Payroll> QueryPayroll(FilterDefinition<Models.Cores.Payroll> filters)
        {
            return Collection.Aggregate().Match(filters).FirstOrDefaultAsync();
        }
    }
}