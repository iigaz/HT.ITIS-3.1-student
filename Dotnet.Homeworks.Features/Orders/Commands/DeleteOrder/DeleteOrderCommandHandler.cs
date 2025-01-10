using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderByGuidCommand>
{
    public DeleteOrderCommandHandler(IOrderRepository orderRepository)
    {
        OrderRepository = orderRepository;
    }

    private IOrderRepository OrderRepository { get; }

    public async Task<Result> Handle(DeleteOrderByGuidCommand request, CancellationToken cancellationToken)
    {
        await OrderRepository.DeleteOrderByGuidAsync(request.Id, cancellationToken);
        return new Result(true);
    }
}