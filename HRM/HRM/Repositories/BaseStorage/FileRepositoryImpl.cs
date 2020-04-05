using System;
using System.Collections;
using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Services.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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
        
        public ObjectId SaveFile(T storageFile)
        {
            _collection.InsertOneAsync(storageFile);
            return storageFile.Id;
        }

        public T FindFileById(string id)
        {
            var objectId = ObjectId.Parse(id);
            return _collection.AsQueryable().Where(x => x.Id.Equals(objectId)).FirstOrDefault();
        }
    }
}