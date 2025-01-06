using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    public UpdateUserCommandHandler(UnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private UnitOfWork UnitOfWork { get; }
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await UnitOfWork.UserRepository.UpdateUserAsync(request.User, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}