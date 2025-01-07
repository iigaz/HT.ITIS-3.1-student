using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Users.PermissionCheckers;

public class ClientPermissionChecker : IPermissionChecker<IClientRequest>
{
    public ClientPermissionChecker(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    private IHttpContextAccessor HttpContextAccessor { get; }

    public Task<PermissionResult> CheckPermissionAsync(IClientRequest request)
    {
        var parsed = Guid.TryParse(
            HttpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
            out var id);
        if (parsed)
            return Task.FromResult(id == request.Guid
                ? new PermissionResult(true)
                : new PermissionResult(false, "You cannot do that."));

        return Task.FromResult(new PermissionResult(false, "Could not parse claims."));
    }
}