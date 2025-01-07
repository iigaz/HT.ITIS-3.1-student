using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQuery : IClientRequest, IQuery<GetUserDto>
{
    public Guid Guid { get; init; }
    public PermissionResult CheckPermission(Guid clientId)
    {
        return clientId == Guid
            ? new PermissionResult(true)
            : new PermissionResult(false, "Users can only view their own profile.");
    }

    public GetUserQuery(Guid guid)
    {
        Guid = guid;
    }
};