using Dotnet.Homeworks.Infrastructure.Utils;

namespace Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

public interface IClientRequest 
{
    public Guid Guid { get; }

    public PermissionResult CheckPermission(Guid clientId);
}