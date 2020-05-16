using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRM.Models.Bases;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HRM.Repositories.Base
{
    public interface IBaseRepository<T> where T: BaseModel
    {
        Task<List<T>> GetAllDocument();
        Task<string> InsertOne(T document);
        Task<bool> UpdateOneById(String id, UpdateDefinition<T> updateDefinition);
        Task<bool> DeleteOneById(String id);
        Task<T> FindFirstById(String id);
        Task<ReplaceOneResult> ReplaceOneById(String id, T newDocument);
    }
}