using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Dto;
using Dotnet.Homeworks.Features.Services;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserDto>
{
    public CreateUserCommandHandler(UnitOfWork unitOfWork, IRegistrationService registrationService)
    {
        UnitOfWork = unitOfWork;
        RegistrationService = registrationService;
    }

    private UnitOfWork UnitOfWork { get; }
    private IRegistrationService RegistrationService { get; }
    
    public async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await RegistrationService.RegisterAsync(new RegisterUserDto(request.Name, request.Email));
        var result = await UnitOfWork.UserRepository.InsertUserAsync(
            new User() { Email = request.Email, Id = Guid.NewGuid(), Name = request.Name }, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result<CreateUserDto>(new CreateUserDto(result), true);
    }
}