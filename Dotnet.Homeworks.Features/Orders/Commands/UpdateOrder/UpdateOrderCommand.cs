using Dotnet.Homeworks.Features.Orders.Permissions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : ICommand, IOrdererRequest
{
    public UpdateOrderCommand(Guid id, IEnumerable<Guid> productsIds)
    {
        Id = id;
        ProductsIds = productsIds;
    }

    public Guid Id { get; init; }
    public IEnumerable<Guid> ProductsIds { get; init; }
}