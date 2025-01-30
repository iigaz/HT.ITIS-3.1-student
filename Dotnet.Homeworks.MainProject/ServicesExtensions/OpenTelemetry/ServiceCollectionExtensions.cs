using Dotnet.Homeworks.MainProject.Configuration;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.OpenTelemetry;

public static class ServiceCollectionExtensions
{
    internal const string ServiceName = "dotnet-homeworks";

    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services,
        OpenTelemetryConfig openTelemetryConfiguration)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ServiceName))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddMeter("Dotnet.Homeworks.Meter")
                .AddConsoleExporter())
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddSource("Dotnet.Homeworks.Source")
                .AddOtlpExporter(options => options.Endpoint = new Uri(openTelemetryConfiguration.OtlpExporterEndpoint))
                .AddConsoleExporter());
        return services;
    }
}