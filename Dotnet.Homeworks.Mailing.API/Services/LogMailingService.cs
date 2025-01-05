using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Dto;
using Dotnet.Homeworks.Shared.Dto;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Dotnet.Homeworks.Mailing.API.Services;

public class LogMailingService : IMailingService
{
    private readonly EmailConfig _emailConfig;
    private readonly ILogger<LogMailingService> _logger;

    public LogMailingService(IOptions<EmailConfig> emailConfig, ILogger<LogMailingService> logger)
    {
        _emailConfig = emailConfig.Value;
        _logger = logger;
    }

    public Task<Result> SendEmailAsync(EmailMessage emailDto)
    {
        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Testing mailing api", _emailConfig.Email));
        message.To.Add(new MailboxAddress(emailDto.Email, emailDto.Email));
        message.Subject = emailDto.Subject ?? "";
        var bodyBuilder = new BodyBuilder
        {
            TextBody = $"Your message: {emailDto.Content}"
        };
        message.Body = bodyBuilder.ToMessageBody();

        _logger.LogInformation("Connect to {host}:{port} with credentials {email} {password}; message: {message}",
            _emailConfig.Host, _emailConfig.Port, _emailConfig.Email, _emailConfig.Password, message);

        return Task.FromResult(new Result(true));
    }
}