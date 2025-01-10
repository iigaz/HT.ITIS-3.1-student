using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Permissions;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.PermissionCheckers;

public class OrdererPermissionChecker: IPermissionChecker<IOrdererRequest>
{
    public OrdererPermissionChecker(IHttpContextAccessor accessor, IUserRepository userRepository, IOrderRepository orderRepository)
    {
        Accessor = accessor;
        UserRepository = userRepository;
        OrderRepository = orderRepository;
    }

    private IHttpContextAccessor Accessor { get; }
    private IUserRepository UserRepository { get; }
    private IOrderRepository OrderRepository { get; }
    public async Task<PermissionResult> CheckPermissionAsync(IOrdererRequest request)
    {
        var parsed = Guid.TryParse(Accessor.HttpContext?.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value, out var userId);
        if (!parsed)
            return new PermissionResult(false, "Could not get user id");
        var order = await OrderRepository.GetOrderByGuidAsync(request.Id, default);
        if (order?.OrdererId != userId)
            return new PermissionResult(false, "User does not own this order.");
        return new PermissionResult(true);
    }
}