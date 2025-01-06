using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetAllUsersDto>
{
    public GetAllUsersQueryHandler(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private UnitOfWork UnitOfWork { get; }
    public async Task<Result<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result =
            await (await UnitOfWork.UserRepository.GetUsersAsync(cancellationToken)).ToArrayAsync(cancellationToken);
        return new Result<GetAllUsersDto>(
            new GetAllUsersDto(result.Select(user => new GetUserDto(user.Id, user.Name, user.Email))), true);
    }
}