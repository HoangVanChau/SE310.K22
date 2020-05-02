using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Services.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HRM.Repositories.Utils
{
    public class AddressRepositoryImpl: IAddressRepository
    {
        private readonly IMongoCollection<Province> _collection;

        public AddressRepositoryImpl(MongoDbService mongoDbService)
        {
            _collection = mongoDbService.GetDb().GetCollection<Province>(Constants.Collections.ProvinceCollection);
        }
        public async Task<List<Locate>> GetProvinces()
        {
            var projection = Builders<Province>
                .Projection
                .Include(x => x.Id)
                .Include(x => x.Name);
            var provinces = await _collection.Find(x => true).Project<Province>(projection).ToListAsync();
            return provinces.ConvertAll(x => new Locate
            {
                LocateId = Int32.Parse(x.Id),
                LocateName = x.Name
            });
        }

        public async Task<List<Locate>> GetDistricts(string provinceId)
        {
            var province = await _collection
                .Find(x => x.Id.Equals(provinceId))
                .FirstOrDefaultAsync();
            if (province == null)
            {
                return new List<Locate>();
            }
            return province.Districts?.ConvertAll(x => new Locate
            {
                LocateId = Int32.Parse(x.Id),
                LocateName = x.Name
            });
        }

        public async Task<List<Locate>> GetWards(string districtId)
        {
            var filter = Builders<Province>.Filter.ElemMatch(p => p.Districts, d => d.Id.Equals(districtId));
            var province = await _collection.Find(filter).FirstOrDefaultAsync();
            if (province == null)
            {
                return new List<Locate>();
            }
            return province.Districts.Find(x => x.Id.Equals(districtId)).Wards.ConvertAll(x => new Locate
            {
                LocateId = Int32.Parse(x.Id),
                LocateName = x.Name
            });
        }
    }
}