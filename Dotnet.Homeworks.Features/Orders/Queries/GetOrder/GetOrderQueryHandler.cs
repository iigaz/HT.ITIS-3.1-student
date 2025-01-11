using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, GetOrderDto>
{
    public GetOrderQueryHandler(IOrderRepository orderRepository, IOrderMapper orderMapper)
    {
        OrderRepository = orderRepository;
        OrderMapper = orderMapper;
    }

    private IOrderRepository OrderRepository { get; }
    private IOrderMapper OrderMapper { get; }

    public async Task<Result<GetOrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await OrderRepository.GetOrderByGuidAsync(request.Id, cancellationToken);
        if (order == null)
            return new Result<GetOrderDto>(default, false, "Order not found.");
        return new Result<GetOrderDto>(OrderMapper.Map(order), true);
    }
}