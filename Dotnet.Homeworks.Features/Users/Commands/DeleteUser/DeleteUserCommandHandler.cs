using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler: ICommandHandler<DeleteUserCommand>
{
    public DeleteUserCommandHandler(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private UnitOfWork UnitOfWork { get; }
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await UnitOfWork.UserRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}