using System;
using System.Threading.Tasks;
using HRM.Models.Cores;
using MongoDB.Bson;

namespace HRM.Repositories.BaseStorage
{
    public interface IFileRepository<T> where T: StorageFile
    {
        public Task<ObjectId> SaveFile(T storageFile);
        public Task<T> FindFileById(String id);
    }
}