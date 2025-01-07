using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;

namespace Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

public interface IAdminRequest
{
    public PermissionResult CheckPermission(Roles role);
}