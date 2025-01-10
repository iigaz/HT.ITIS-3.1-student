using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, GetOrderDto>
{
    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        OrderRepository = orderRepository;
    }

    private IOrderRepository OrderRepository { get; }

    public async Task<Result<GetOrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await OrderRepository.GetOrderByGuidAsync(request.Id, cancellationToken);
        if (order == null)
            return new Result<GetOrderDto>(default, false, "Order not found.");
        return new Result<GetOrderDto>(new GetOrderDto(order.Id, order.ProductsIds), true);
    }
}