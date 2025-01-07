using Dotnet.Homeworks.Features.Services;
using Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.MainProject.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Features.ServiceExtensions;

public static class FeaturesExtensions
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        // Validators
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserValidator>();
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserValidator>();

        // Services
        services.AddSingleton<IRegistrationService, RegistrationService>();
        services.AddSingleton<ICommunicationService, CommunicationService>();

        return services;
    }
}