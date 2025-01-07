using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

public class ServiceMediator : IMediator
{
    public ServiceMediator(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }
    
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var handler =
            scope.ServiceProvider.GetService(typeof(IRequestHandler<IRequest<TResponse>, TResponse>)) as
                IRequestHandler<IRequest<TResponse>, TResponse>;
        if (handler == null)
            throw new NotSupportedException(
                $"Request handler for type {typeof(IRequestHandler<IRequest<TResponse>, TResponse>)} was not registered.");
        return await handler.Handle(request, cancellationToken);
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var handler =
            scope.ServiceProvider.GetService(typeof(IRequestHandler<TRequest>)) as
                IRequestHandler<TRequest>;
        if (handler == null)
            throw new NotSupportedException(
                $"Request handler for type {typeof(IRequestHandler<TRequest>)} was not registered.");
        await handler.Handle(request, cancellationToken);
    }

    public async Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetService(request.GetType());
        return handler == null ? null : await handler.Handle(request, cancellationToken);
    }
}