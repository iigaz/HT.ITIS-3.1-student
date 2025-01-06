using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserDto>
{
    public GetUserQueryHandler(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private UnitOfWork UnitOfWork { get; }
    public async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.UserRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
        return result == null
            ? new Result<GetUserDto>(null, false, "User not found.")
            : new Result<GetUserDto>(new GetUserDto(result.Id, result.Name, result.Email), true);
    }
}