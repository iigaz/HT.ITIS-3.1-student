using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Features.ServiceExtensions;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.MainProject.Configuration;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Mapper;
using Dotnet.Homeworks.MainProject.ServicesExtensions.Masstransit;
using Dotnet.Homeworks.MainProject.ServicesExtensions.MongoDb;
using Dotnet.Homeworks.MainProject.ServicesExtensions.OpenTelemetry;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMediator(AssemblyReference.Assembly);
builder.Services.AddMasstransitRabbitMq(builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>()!);
builder.Services.AddMongoClient(builder.Configuration.GetSection("MongoDb").Get<MongoDbConfig>()!);
builder.Services.AddMappers(AssemblyReference.Assembly);
builder.Services.AddOpenTelemetry(builder.Configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConfig>()!);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddFeatures();
builder.Services.AddPermissionChecks(AssemblyReference.Assembly);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapControllers();

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
}

app.Run();