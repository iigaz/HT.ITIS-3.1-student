using Dotnet.Homeworks.Shared.MessagingContracts.Email;

namespace Dotnet.Homeworks.Features.Services;

public interface ICommunicationService
{
    public Task SendEmailAsync(SendEmail sendEmailDto);
}