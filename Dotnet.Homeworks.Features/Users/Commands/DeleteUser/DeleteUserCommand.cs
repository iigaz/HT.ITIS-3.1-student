using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IClientRequest, ICommand
{
    public Guid Guid { get; }

    public DeleteUserCommand(Guid guid)
    {
        Guid = guid;
    }
}