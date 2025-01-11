using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Mapster;

namespace Dotnet.Homeworks.Features.UserManagement.Mapping;

public class RegisterUserManagementMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetUserDto>()
            .Map(dto => dto.Guid, user => user.Id)
            .RequireDestinationMemberSource(true);
    }
}