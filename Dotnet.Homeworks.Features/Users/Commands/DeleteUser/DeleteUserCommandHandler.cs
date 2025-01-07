using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler: CqrsDecorator<DeleteUserCommand, object>, ICommandHandler<DeleteUserCommand>
{
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IPermissionCheck permissionCheck): base(permissionCheck, null)
    {
        UnitOfWork = unitOfWork;
    }

    private IUnitOfWork UnitOfWork { get; }
    public new async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var decResult = await base.Handle(request, cancellationToken);
        if (decResult.IsFailure)
            return decResult;
        await UnitOfWork.UserRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}