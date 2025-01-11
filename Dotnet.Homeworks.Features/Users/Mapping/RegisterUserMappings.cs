using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Mapster;

namespace Dotnet.Homeworks.Features.Users.Mapping;

public class RegisterUserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetUserDto>()
            .Map(dto => dto.Guid, user => user.Id)
            .RequireDestinationMemberSource(true);
    }
}