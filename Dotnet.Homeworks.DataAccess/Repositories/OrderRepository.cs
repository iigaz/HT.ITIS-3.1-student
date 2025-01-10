using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using MongoDB.Driver;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private const string DatabaseName = "OrderDatabase";
    private const string CollectionName = "OrderCollection";

    public OrderRepository(MongoClient mongoClient)
    {
        var db = mongoClient.GetDatabase(DatabaseName)!;
        OrdersCollection = db.GetCollection<Order>(CollectionName);
    }

    private IMongoCollection<Order> OrdersCollection { get; }

    public async Task<IEnumerable<Order>> GetAllOrdersFromUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await OrdersCollection.Find(order => order.OrdererId == userId).ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await OrdersCollection.Find(order => order.Id == orderId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        await OrdersCollection.DeleteOneAsync(order => order.Id == orderId, cancellationToken);
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await OrdersCollection.ReplaceOneAsync(ord => ord.Id == order.Id, order, cancellationToken: cancellationToken);
    }

    public async Task<Guid> InsertOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await OrdersCollection.InsertOneAsync(order, cancellationToken: cancellationToken);
        return order.Id;
    }
}