using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IClientRequest, ICommand
{
    public User User { get; }
    
    public Guid Guid { get; }
    public PermissionResult CheckPermission(Guid clientId)
    {
        return clientId == Guid
            ? new PermissionResult(true)
            : new PermissionResult(false, "Users can only update their own profile.");
    }

    public UpdateUserCommand(User user)
    {
        Guid = user.Id;
        User = user;
    }

}