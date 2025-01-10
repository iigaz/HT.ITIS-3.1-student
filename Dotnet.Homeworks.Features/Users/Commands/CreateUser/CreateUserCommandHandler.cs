using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Dto;
using Dotnet.Homeworks.Features.Services;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.Decorators;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : CqrsDecorator<CreateUserCommand, CreateUserDto>,
    ICommandHandler<CreateUserCommand, CreateUserDto>
{
    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IRegistrationService registrationService,
        IPermissionCheck permissionCheck, IValidator<CreateUserCommand>? validator) : base(permissionCheck, validator)
    {
        UnitOfWork = unitOfWork;
        RegistrationService = registrationService;
    }

    private IUnitOfWork UnitOfWork { get; }
    private IRegistrationService RegistrationService { get; }

    public new async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var decResult = await base.Handle(request, cancellationToken);
        if (decResult.IsFailure)
            return decResult;
        await RegistrationService.RegisterAsync(new RegisterUserDto(request.Name, request.Email));
        var userId = Guid.NewGuid();
        await UnitOfWork.UserRepository.InsertUserAsync(
            new User() { Email = request.Email, Id = userId, Name = request.Name }, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result<CreateUserDto>(new CreateUserDto(userId), true);
    }
}