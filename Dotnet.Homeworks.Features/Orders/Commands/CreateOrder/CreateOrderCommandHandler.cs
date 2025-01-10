using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderDto>
{
    private IHttpContextAccessor Accessor { get; }

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IHttpContextAccessor accessor)
    {
        OrderRepository = orderRepository;
        Accessor = accessor;
    }

    private IOrderRepository OrderRepository { get; }

    public async Task<Result<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();
        var parsed = Guid.TryParse(Accessor.HttpContext?.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value, out var ordererId);
        if (!parsed)
            return new Result<CreateOrderDto>(default, false, "Could not get orderer Id");
        await OrderRepository.InsertOrderAsync(new Order()
            { Id = orderId, OrdererId = ordererId, ProductsIds = request.ProductsIds }, cancellationToken);
        return new Result<CreateOrderDto>(new CreateOrderDto(orderId), true);
    }
}