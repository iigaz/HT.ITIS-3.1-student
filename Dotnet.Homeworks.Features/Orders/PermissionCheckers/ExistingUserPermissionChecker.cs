using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using Dotnet.Homeworks.Features.Orders.Permissions;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.PermissionCheckers;

public class ExistingUserPermissionChecker : IPermissionChecker<IExistingUserRequest>
{
    public ExistingUserPermissionChecker(IHttpContextAccessor accessor, IUserRepository userRepository)
    {
        Accessor = accessor;
        UserRepository = userRepository;
    }

    private IHttpContextAccessor Accessor { get; }
    private IUserRepository UserRepository { get; }
    public async Task<PermissionResult> CheckPermissionAsync(IExistingUserRequest request)
    {
        var userIdStr = Accessor.HttpContext?.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var parsed = Guid.TryParse(userIdStr, out var userId);
        if (!parsed)
            return new PermissionResult(false, "Could not get user id");
        var user = await UserRepository.GetUserByGuidAsync(userId, default);
        if (user == null)
            return new PermissionResult(false, "Could not find user.");
        return new PermissionResult(true);
    }
}