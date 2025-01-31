using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Services;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.MainProject.Controllers;
using Dotnet.Homeworks.MainProject.Services;

namespace Dotnet.Homeworks.Tests.Cqrs.Helpers;

internal class CqrsEnvironment
{
    public CqrsEnvironment(ProductManagementController productManagementController, IUnitOfWork unitOfWorkMock,
        MediatR.IMediator mediatR, Mediator.IMediator customMediator,
        IUserRepository userRepository, IRegistrationService registrationService)
    {
        ProductManagementController = productManagementController;
        CustomMediator = customMediator;
        UserRepository = userRepository;
        MediatR = mediatR;
        UnitOfWorkMock = unitOfWorkMock;
        RegistrationService = registrationService;
    }

    public ProductManagementController ProductManagementController { get; }
    public IUnitOfWork UnitOfWorkMock { get; }
    public MediatR.IMediator MediatR { get; }
    public Mediator.IMediator CustomMediator { get; }
    public IUserRepository UserRepository { get; }
    public IRegistrationService RegistrationService { get; }
}