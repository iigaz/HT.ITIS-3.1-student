using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersDto>
{
    private IHttpContextAccessor Accessor { get; }

    public GetOrdersQueryHandler(IOrderRepository orderRepository, IHttpContextAccessor accessor)
    {
        OrderRepository = orderRepository;
        Accessor = accessor;
    }

    private IOrderRepository OrderRepository { get; }

    public async Task<Result<GetOrdersDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var parsed = Guid.TryParse(Accessor.HttpContext?.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value, out var ordererId);
        if (!parsed)
            return new Result<GetOrdersDto>(default, false, "Could not get orderer Id");

        var orders = await OrderRepository.GetAllOrdersFromUserAsync(ordererId, cancellationToken);
        return new Result<GetOrdersDto>(
            new GetOrdersDto(orders.Select(order => new GetOrderDto(order.Id, order.ProductsIds))), true);
    }
}