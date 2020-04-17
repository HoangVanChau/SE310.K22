using System;
using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Services.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HRM.Repositories.BaseStorage
{
    public abstract class FileRepositoryImpl<T>: IFileRepository<T> where T: StorageFile
    {
        protected readonly IMongoCollection<T> _collection;

        public FileRepositoryImpl(MongoDbService mongoDbService)
        {
            _collection = mongoDbService.GetStorageDb().GetCollection<T>(GetCollectionName());
        }

        public abstract String GetCollectionName();
        
        public async Task<ObjectId> SaveFile(T storageFile)
        {
            await _collection.InsertOneAsync(storageFile);
            return storageFile.Id;
        }

        public async Task<T> FindFileById(string id)
        {
            var objectId = ObjectId.Parse(id);
            return await _collection.Find(x => x.Id.Equals(objectId)).FirstOrDefaultAsync();
        }
    }
}