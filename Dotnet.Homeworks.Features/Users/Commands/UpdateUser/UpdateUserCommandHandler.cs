using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : CqrsDecorator<UpdateUserCommand, object>, ICommandHandler<UpdateUserCommand>
{
    public UpdateUserCommandHandler(UnitOfWork unitOfWork, IPermissionCheck permissionCheck, IValidator<UpdateUserCommand>? validator) : base(permissionCheck, validator)
    {
        UnitOfWork = unitOfWork;
    }

    private UnitOfWork UnitOfWork { get; }
    public new async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var decResult = await base.Handle(request, cancellationToken);
        if (decResult.IsFailure)
            return decResult;
        await UnitOfWork.UserRepository.UpdateUserAsync(request.User, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}