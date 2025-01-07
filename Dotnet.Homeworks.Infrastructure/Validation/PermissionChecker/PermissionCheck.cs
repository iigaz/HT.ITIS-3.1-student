using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public class PermissionCheck : IPermissionCheck
{
    public PermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    private IHttpContextAccessor HttpContextAccessor { get; }

    public Task<IEnumerable<PermissionResult>> CheckPermissionAsync<TRequest>(TRequest request)
    {
        var parsed = Enum.TryParse<Roles>(HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role),
                         out var role) &
                     Guid.TryParse(HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
                         out var id);
        if (!parsed)
            return Task.FromResult<IEnumerable<PermissionResult>>(new[]
                { new PermissionResult(false, "Could not parse claims.") });
        var ans = new List<PermissionResult>();
        if (request is IAdminRequest adminRequest)
            ans.Add(adminRequest.CheckPermission(role));
        if (request is IClientRequest clientRequest)
            ans.Add(clientRequest.CheckPermission(id));
        return Task.FromResult<IEnumerable<PermissionResult>>(ans);
    }
}