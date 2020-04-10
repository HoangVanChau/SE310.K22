using System.Threading.Tasks;
using HRM.Constants;
using HRM.Helpers;
using HRM.Models.Cores;
using MongoDB.Driver;

namespace HRM.Services.MongoDB
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoDatabase _storageDb;

        public MongoDbService(IMongoDbSetting setting)
        {
            var credential = MongoCredential.CreateCredential(setting.Database, setting.UserName, setting.Password);
            var storageCredential = MongoCredential.CreateCredential(setting.StorageDatabase, setting.UserName, setting.Password);
            
            var settings = new MongoClientSettings
            {
                Credential = credential,
                Server = new MongoServerAddress(setting.Host, setting.Port)
            };
            var storageSettings = new MongoClientSettings
            {
                Credential = storageCredential,
                Server = new MongoServerAddress(setting.Host, setting.Port)
            };
            
            var client = new MongoClient(settings);
            var storageClient = new MongoClient(storageSettings);
            
            _db = client.GetDatabase(setting.Database);
            _storageDb = storageClient.GetDatabase(setting.StorageDatabase);
        }

        public IMongoDatabase GetDb() => _db;
        public IMongoDatabase GetStorageDb() => _storageDb;

        public async Task ValidateDb()
        {
            //count user for employee Id increase
            var counterCollection = GetDb().GetCollection<Counter>(Collections.CounterCollection);
            if (counterCollection.CountDocuments(x => true) == 0)
            {
                await counterCollection.InsertOneAsync(new Counter
                {
                    Name = Collections.UserCollection,
                    Value = 0
                });
            }
        }
    }
}