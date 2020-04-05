using System;
using System.Threading.Tasks;
using HRM.Models.Cores;
using HRM.Repositories.BaseStorage;

namespace HRM.Repositories.Image
{
    public interface IImageRepository: IFileRepository<ImageFile>
    {
        
    }
}