using HRM.Helpers;
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
    }
}