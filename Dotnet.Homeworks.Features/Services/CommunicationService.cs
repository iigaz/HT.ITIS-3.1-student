using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.Features.Services;

public class CommunicationService : ICommunicationService
{
    public CommunicationService(IBus bus)
    {
        Bus = bus;
    }

    private IBus Bus { get; }
    public Task SendEmailAsync(SendEmail sendEmailDto)
    {
        Bus.Publish(sendEmailDto);
        return Task.CompletedTask;
    }
}