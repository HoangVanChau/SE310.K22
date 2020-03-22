using System;
using System.Collections.Generic;
using HRM.Models.Base;
using HRM.Services.MongoDB;
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
        public List<T> GetAllDocument()
        {
            return _collection.AsQueryable().ToList();
        }

        public bool InsertOne(T document)
        {
            document.CreatedDate = DateTime.Now;
            document.ModifyDate = new List<DateTime>();
            
            _collection.InsertOne(document);
            return true;
        }

        public bool UpdateOneById(string id, UpdateDefinition<T> updateDefinition)
        {
            updateDefinition.Push(e => e.ModifyDate, DateTime.Now);
            return _collection.UpdateOne(x => x.Id.Equals(id), updateDefinition).ModifiedCount.Equals(1);
        }

        public bool DeleteOneById(String id)
        {
            return _collection.DeleteOne(x => x.Id.Equals(id)).DeletedCount.Equals(1);
        }

        public T FindFirstById(string id)
        {
            return _collection.Find(x => x.Id.Equals(id)).FirstOrDefault();
        }
    }
}