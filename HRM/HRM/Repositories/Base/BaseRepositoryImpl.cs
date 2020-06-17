using System;
using System.Collections.Generic;
using System.Linq;
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
        protected readonly IMongoCollection<T> Collection;

        public BaseRepositoryImpl(MongoDbService service)
        {
            Collection = service.GetDb().GetCollection<T>(GetCollectionName());
        }

        public abstract string GetCollectionName();
        
        public async Task<List<T>> GetAllDocument()
        {
            return await Collection.Find(x => true).ToListAsync();
        }

        public async Task<string> InsertOne(T document)
        {
            var now = DateTime.Now;
            document.CreatedDate = now;
            document.ModifyDate = new List<DateTime>{now};
            
            await Collection.InsertOneAsync(document);
            return document.Id;
        }

        public async Task<List<string>> InsertMany(List<T> documents)
        {
            var now = DateTime.Now;
            foreach (var document in documents)
            {
                document.CreatedDate = now;
                document.ModifyDate = new List<DateTime>{now};
            }

            await Collection.InsertManyAsync(documents);
            return documents.Select(x => x.Id).ToList();
        }

        public async Task<bool> UpdateOneById(string id, UpdateDefinition<T> updateDefinition)
        {
            var finalDefinition = updateDefinition.Push(e => e.ModifyDate, DateTime.Now);
            var task = await Collection.UpdateOneAsync(x => x.Id.Equals(id), finalDefinition);
            return task.ModifiedCount.Equals(1);
        }

        public async Task<bool> DeleteOneById(String id)
        {
            var task = await Collection.DeleteOneAsync(x => x.Id.Equals(id));
            return task.DeletedCount.Equals(1);
        }

        public async Task<T> FindFirstById(string id)
        {
            return await Collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public Task<ReplaceOneResult> ReplaceOneById(string id, T newDocument)
        {
            return Collection.ReplaceOneAsync(x => x.Id.Equals(id), newDocument);
        }

        public Task<T> QueryOne(FilterDefinition<T> filter)
        {
            return Collection.Aggregate().Match(filter).FirstOrDefaultAsync();
        }

        public Task<List<T>> QueryMany(FilterDefinition<T> filter)
        {
            return Collection.Aggregate().Match(filter).ToListAsync();
        }
    }
}