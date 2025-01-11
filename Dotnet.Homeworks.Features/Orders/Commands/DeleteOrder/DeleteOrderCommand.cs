using Dotnet.Homeworks.Features.Orders.Permissions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderByGuidCommand : ICommand, IOrdererRequest
{
    public DeleteOrderByGuidCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }
}