using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;

public class DeleteUserByAdminCommandHandler : ICommandHandler<DeleteUserByAdminCommand>
{
    public DeleteUserByAdminCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        UnitOfWork = unitOfWork;
        UserRepository = userRepository;
    }

    private IUnitOfWork UnitOfWork { get; }
    private IUserRepository UserRepository { get; }
    public async Task<Result> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
    {
        if (!(await (await UserRepository.GetUsersAsync(cancellationToken)).AnyAsync(user => user.Id == request.Guid, cancellationToken: cancellationToken)))
            return new Result(false, "No such user found.");
        await UnitOfWork.UserRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}