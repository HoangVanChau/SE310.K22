using HRM.Models.Cores;
using HRM.Repositories.Base;
using HRM.Services.MongoDB;

namespace HRM.Repositories
{
    public class HolidayRepositoryImpl : BaseRepositoryImpl<Holiday>, IHolidayRepository
    {
        public HolidayRepositoryImpl(MongoDbService service) : base(service)
        {
        }

        public override string GetCollectionName()
        {
            return Constants.Collections.HolidayCollection;
        }
    }
}