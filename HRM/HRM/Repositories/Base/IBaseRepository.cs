using System;
using System.Collections.Generic;
using HRM.Models.Base;
using MongoDB.Driver;

namespace HRM.Repositories.Base
{
    public interface IBaseRepository<T> where T: BaseModel
    {
        List<T> GetAllDocument();
        bool InsertOne(T document);
        bool UpdateOneById(String id, UpdateDefinition<T> updateDefinition);
        bool DeleteOneById(String id);
        T FindFirstById(String id);
    }
}