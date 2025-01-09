using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;

namespace Dotnet.Homeworks.Storage.API.Services;

public class StorageFactory : IStorageFactory
{
    public StorageFactory(IMinioClient minioClient)
    {
        MinioClient = minioClient;
    }

    private IMinioClient MinioClient { get; }
    public Task<IStorage<Image>> CreateImageStorageWithinBucketAsync(string bucketName)
    {
        return Task.FromResult<IStorage<Image>>(new ImageStorage(MinioClient, bucketName));
    }
}