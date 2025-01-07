using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : CqrsDecorator<GetUserQuery, GetUserDto>, IQueryHandler<GetUserQuery, GetUserDto>
{
    public GetUserQueryHandler(IUnitOfWork unitOfWork, IPermissionCheck permissionCheck) : base(permissionCheck, null)
    {
        UnitOfWork = unitOfWork;
    }

    private IUnitOfWork UnitOfWork { get; }
    public new async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var decResult = await base.Handle(request, cancellationToken);
        if (decResult.IsFailure)
            return decResult;
        var result = await UnitOfWork.UserRepository.GetUserByGuidAsync(request.Guid, cancellationToken);
        return result == null
            ? new Result<GetUserDto>(null, false, "User not found.")
            : new Result<GetUserDto>(new GetUserDto(result.Id, result.Name, result.Email), true);
    }
}