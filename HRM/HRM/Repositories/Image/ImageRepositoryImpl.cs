using System;
using HRM.Models.Cores;
using HRM.Repositories.BaseStorage;
using HRM.Services.MongoDB;

namespace HRM.Repositories.Image
{
    public class ImageRepositoryImpl: FileRepositoryImpl<ImageFile>, IImageRepository
    {
        public const String ImageCollectionName = "Image";
        
        public ImageRepositoryImpl(MongoDbService mongoDbService) : base(mongoDbService)
        {
            
        }

        public override string GetCollectionName()
        {
            return ImageCollectionName;
        }
    }
}