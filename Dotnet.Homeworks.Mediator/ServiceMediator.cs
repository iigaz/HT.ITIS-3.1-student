namespace Dotnet.Homeworks.Mediator;

public class ServiceMediator : IMediator
{
    public ServiceMediator(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }
    
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handler =
            ServiceProvider.GetService(typeof(IRequestHandler<IRequest<TResponse>, TResponse>)) as
                IRequestHandler<IRequest<TResponse>, TResponse>;
        if (handler == null)
            throw new NotSupportedException(
                $"Request handler for type {typeof(IRequest<TResponse>)} was not registered.");
        return handler.Handle(request, cancellationToken);
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        var handler =
            ServiceProvider.GetService(typeof(IRequestHandler<TRequest>)) as
                IRequestHandler<TRequest>;
        if (handler == null)
            throw new NotSupportedException(
                $"Request handler for type {typeof(TRequest)} was not registered.");
        return handler.Handle(request, cancellationToken);
    }

    public Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        var handler = ServiceProvider.GetService(request.GetType());
        return handler == null ? Task.FromResult<object?>(null) : (Task<dynamic?>)handler.Handle(request, cancellationToken);
    }
}