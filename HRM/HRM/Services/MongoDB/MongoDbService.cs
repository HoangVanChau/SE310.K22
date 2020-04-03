using HRM.Helpers;
using MongoDB.Driver;

namespace HRM.Services.MongoDB
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _db;

        public MongoDbService(IMongoDbSetting setting)
        {
            var credential = MongoCredential.CreateCredential(setting.Database, setting.UserName, setting.Password);
            var settings = new MongoClientSettings
            {
                Credential = credential,
                Server = new MongoServerAddress(setting.Host, setting.Port)
            };
            var client = new MongoClient(settings);
            _db = client.GetDatabase(setting.Database);
        }

        public IMongoDatabase GetDb() => _db;
    }
}