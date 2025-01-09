using System.Reactive.Linq;
using Minio;
namespace Dotnet.Homeworks.Storage.API.Services;

public class PendingObjectsProcessor : BackgroundService
{
    public PendingObjectsProcessor(IMinioClient minioClient)
    {
        MinioClient = minioClient;
    }

    private IMinioClient MinioClient { get; }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Constants.PendingObjectProcessor.Period);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
            await MoveObjectsToDestination(stoppingToken);
    }

    private async Task MoveObjectsToDestination(CancellationToken cancellationToken)
    {
        var args = new BucketExistsArgs()
            .WithBucket(Constants.Buckets.Pending);
        var found = await MinioClient.BucketExistsAsync(args, cancellationToken);
        if (!found) return;
        var listArgs = new ListObjectsArgs().WithBucket(Constants.Buckets.Pending);
        var observable = MinioClient.ListObjectsAsync(listArgs, cancellationToken);
        var items = observable.Select(item => item.Key).ToList().Wait();
        foreach (var item in items)
        {
            if (cancellationToken.IsCancellationRequested)
                break;
            var objectStatArgs = new StatObjectArgs()
                .WithBucket(Constants.Buckets.Pending)
                .WithObject(item);
            var statObject = await MinioClient.StatObjectAsync(objectStatArgs, cancellationToken);
            if (!statObject.MetaData.ContainsKey(Constants.MetadataKeys.Destination) || statObject.MetaData[Constants.MetadataKeys.Destination] == Constants.Buckets.Pending) continue;
            var copySourceObjectArgs = new CopySourceObjectArgs()
                .WithBucket(Constants.Buckets.Pending)
                .WithObject(item);
            var copyObjectArgs = new CopyObjectArgs()
                .WithBucket(statObject.MetaData[Constants.MetadataKeys.Destination])
                .WithObject(item)
                .WithCopyObjectSource(copySourceObjectArgs);
            await MinioClient.CopyObjectAsync(copyObjectArgs, cancellationToken);
        }
        if (items.Count > 0 )
        {
            var removeArgs = new RemoveObjectsArgs()
                .WithBucket(Constants.Buckets.Pending)
                .WithObjects(items);
            await MinioClient.RemoveObjectsAsync(removeArgs, cancellationToken);
        }
    }
}