using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public class GetAllUsersQuery : IAdminRequest, IQuery<GetAllUsersDto>
{
    public PermissionResult CheckPermission(Roles role)
    {
        return role == Roles.Admin
            ? new PermissionResult(true)
            : new PermissionResult(false, "Only admins can get all users.");
    }
}