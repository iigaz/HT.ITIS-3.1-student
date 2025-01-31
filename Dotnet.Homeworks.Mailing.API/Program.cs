using Dotnet.Homeworks.Mailing.API.Configuration;
using Dotnet.Homeworks.Mailing.API.Services;
using Dotnet.Homeworks.Mailing.API.ServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));

builder.Services.AddMasstransitRabbitMq(builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>()!);

builder.Services.AddScoped<IMailingService, LogMailingService>();

var app = builder.Build();

app.Run();