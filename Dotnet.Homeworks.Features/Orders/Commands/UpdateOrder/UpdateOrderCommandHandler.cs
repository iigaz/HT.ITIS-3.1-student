using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
{
    public UpdateOrderCommandHandler(IOrderRepository orderRepository)
    {
        OrderRepository = orderRepository;
    }

    private IOrderRepository OrderRepository { get; }

    public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await OrderRepository.GetOrderByGuidAsync(request.Id, cancellationToken);
        if (order == null)
            return new Result(false, "Order not found.");
        await OrderRepository.UpdateOrderAsync(
            new Order() { Id = request.Id, OrdererId = order.OrdererId, ProductsIds = request.ProductsIds },
            cancellationToken);
        return new Result(true);
    }
}