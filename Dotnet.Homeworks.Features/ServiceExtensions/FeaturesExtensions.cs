using Dotnet.Homeworks.Features.Services;
using Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.MainProject.Services;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Features.ServiceExtensions;

public static class FeaturesExtensions
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {

        // Validators
        services.AddValidatorsFromAssembly(Helpers.AssemblyReference.Assembly);

        // Services
        services.AddSingleton<IRegistrationService, RegistrationService>();
        services.AddSingleton<ICommunicationService, CommunicationService>();
        return services;
    }
}