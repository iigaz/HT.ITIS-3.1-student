using System.Reactive.Linq;
using Dotnet.Homeworks.Shared.Dto;
using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;
using Minio.Exceptions;

namespace Dotnet.Homeworks.Storage.API.Services;

public class ImageStorage : IStorage<Image>
{
    private string Bucket { get; }
    private IMinioClient MinioClient { get; }

    public ImageStorage(IMinioClient minioClient, string bucket)
    {
        Bucket = bucket;
        MinioClient = minioClient;
    }

    private async Task CreateBucketIfNotExists(string bucketName)
    {
        var args = new BucketExistsArgs().WithBucket(bucketName);
        var found = await MinioClient.BucketExistsAsync(args);
        if (!found)
        {
            var makeArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await MinioClient.MakeBucketAsync(makeArgs);
        }
    }
    
    public async Task<Result> PutItemAsync(Image item, CancellationToken cancellationToken = default)
    {
        await CreateBucketIfNotExists(Constants.Buckets.Pending);
        await CreateBucketIfNotExists(Bucket);
        var statArgs = new StatObjectArgs().WithBucket(Bucket).WithObject(item.FileName);
        try
        {
            await MinioClient.StatObjectAsync(statArgs, cancellationToken);
            return new Result(false, "Object already exists.");
        }
        catch (ObjectNotFoundException)
        {
            // Just what we need
        }
        statArgs = new StatObjectArgs().WithBucket(Constants.Buckets.Pending).WithObject(item.FileName);
        try
        {
            await MinioClient.StatObjectAsync(statArgs, cancellationToken);
            return new Result(false, "Operation is already pending.");
        }
        catch (ObjectNotFoundException)
        {
            // Just what we need
        }
        
        item.Metadata.TryAdd(Constants.MetadataKeys.Destination, Bucket);
        var args = new PutObjectArgs()
            .WithBucket(Constants.Buckets.Pending)
            .WithObject(item.FileName)
            .WithStreamData(item.Content)
            .WithObjectSize(item.Content.Length)
            .WithContentType(item.ContentType)
            .WithHeaders(item.Metadata);
        try
        {
            _ = await MinioClient.PutObjectAsync(args, cancellationToken);
        }
        catch (MinioException minioException)
        {
            return new Result(false, minioException.Message);
        }
        return new Result(true);
    }

    public async Task<Image?> GetItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var content = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(Bucket)
            .WithObject(itemName)
            .WithCallbackStream(async (stream, token) => await stream.CopyToAsync(content, token));
        try
        {
            var stat = await MinioClient.GetObjectAsync(args, cancellationToken);
            return new Image(content, stat.ObjectName, stat.ContentType, stat.MetaData);
        }
        catch (MinioException _)
        {
            return null;
        }
    }

    public async Task<Result> RemoveItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(Bucket)
            .WithObject(itemName);
        try
        {
            await MinioClient.RemoveObjectAsync(args, cancellationToken);
            return new Result(true);
        }
        catch (MinioException minioException)
        {
            if (minioException is ObjectNotFoundException or BucketNotFoundException)
                return new Result(true);
            return new Result(false, minioException.Message);
        }
    }

    public async Task<IEnumerable<string>> EnumerateItemNamesAsync(CancellationToken cancellationToken = default)
    {
        await CreateBucketIfNotExists(Bucket);
        var listArgs = new ListObjectsArgs().WithBucket(Bucket);
        var observable = MinioClient.ListObjectsAsync(listArgs, cancellationToken);
        return observable.Select(item => item.Key).ToList().Wait();
    }

    public async Task<Result> CopyItemToBucketAsync(string itemName, string destinationBucketName,
        CancellationToken cancellationToken = default)
    {
        await CreateBucketIfNotExists(destinationBucketName);
        var copySourceObjectArgs = new CopySourceObjectArgs()
            .WithBucket(Bucket)
            .WithObject(itemName);
        var copyObjectArgs = new CopyObjectArgs()
            .WithBucket(destinationBucketName)
            .WithObject(itemName)
            .WithCopyObjectSource(copySourceObjectArgs);
        try
        {
            await MinioClient.CopyObjectAsync(copyObjectArgs, cancellationToken);
            return new Result(true);
        }
        catch (MinioException minioException)
        {
            return new Result(false, minioException.Message);
        }
    }
}