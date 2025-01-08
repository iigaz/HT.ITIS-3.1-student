using Dotnet.Homeworks.Features.Dto;

namespace Dotnet.Homeworks.Features.Services;

public interface IRegistrationService
{
    public Task RegisterAsync(RegisterUserDto userDto);
}