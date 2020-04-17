using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Models.Bases;
using HRM.Services.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace HRM.Repositories.Base
{
    public abstract class BaseRepositoryImpl<T>: IBaseRepository<T> where T: BaseModel
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseRepositoryImpl(MongoDbService service)
        {
            _collection = service.GetDb().GetCollection<T>(GetCollectionName());
        }
        
        public abstract string GetCollectionName();
        public async Task<List<T>> GetAllDocument()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<ObjectId> InsertOne(T document)
        {
            document.CreatedDate = DateTime.Now;
            document.ModifyDate = new List<DateTime>();
            
            await _collection.InsertOneAsync(document);
            return document.Id;
        }

        public async Task<bool> UpdateOneById(string id, UpdateDefinition<T> updateDefinition)
        {
            updateDefinition.Push(e => e.ModifyDate, DateTime.Now);
            var task = await _collection.UpdateOneAsync(x => x.Id.Equals(ObjectId.Parse(id)), updateDefinition);
            return task.ModifiedCount.Equals(1);
        }

        public async Task<bool> DeleteOneById(String id)
        {
            var task = await _collection.DeleteOneAsync(x => x.Id.Equals(ObjectId.Parse(id)));
            return task.DeletedCount.Equals(1);
        }

        public async Task<T> FindFirstById(string id)
        {
            return await _collection.Find(x => x.Id.Equals(ObjectId.Parse(id))).FirstOrDefaultAsync();
        }
    }
}