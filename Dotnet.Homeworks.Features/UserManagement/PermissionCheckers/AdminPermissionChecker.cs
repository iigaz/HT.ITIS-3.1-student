using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.UserManagement.PermissionCheckers;

public class AdminPermissionChecker : IPermissionChecker<IAdminRequest>
{
    public AdminPermissionChecker(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    private IHttpContextAccessor HttpContextAccessor { get; }

    public Task<PermissionResult> CheckPermissionAsync(IAdminRequest request)
    {
        var parsed = Enum.TryParse<Roles>(HttpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value,
            out var role);
        if (!parsed)
            return Task.FromResult(new PermissionResult(false, "Could not parse claims."));
        return Task.FromResult(role == Roles.Admin
            ? new PermissionResult(true)
            : new PermissionResult(false, "Not enough privileges."));
    }
}