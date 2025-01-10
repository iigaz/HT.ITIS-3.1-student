using Dotnet.Homeworks.Features.Orders.Permissions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : ICommand<CreateOrderDto>, IExistingUserRequest
{
    public CreateOrderCommand(IEnumerable<Guid> productsIds)
    {
        ProductsIds = productsIds;
    }

    public IEnumerable<Guid> ProductsIds { get; init; }
}