using Dotnet.Homeworks.Mailing.API.Dto;
using Dotnet.Homeworks.Mailing.API.Services;
using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.Mailing.API.Consumers;

public class EmailConsumer : IEmailConsumer
{
    public EmailConsumer(IMailingService mailingService)
    {
        MailingService = mailingService;
    }

    private IMailingService MailingService { get; }
    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        Console.WriteLine("Received message");
        await MailingService.SendEmailAsync(new EmailMessage(context.Message.ReceiverEmail, context.Message.Subject, context.Message.Content));
    }
}