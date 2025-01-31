using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetAllUsersDto>
{
    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    private IUserRepository UserRepository { get; }

    public async Task<Result<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result =
            await (await UserRepository.GetUsersAsync(cancellationToken)).ProjectToType<GetUserDto>()
                .ToArrayAsync(cancellationToken);
        return new Result<GetAllUsersDto>(new GetAllUsersDto(result), true);
    }
}