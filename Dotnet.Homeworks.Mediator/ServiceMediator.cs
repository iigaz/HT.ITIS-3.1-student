using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

public class ServiceMediator : IMediator
{
    private ConcurrentBag<Type> RegisteredRequestsImpl { get; } = new ConcurrentBag<Type>();
    public ServiceMediator(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    internal void AddRequestImpl(Type requestTypeType)
    {
        RegisteredRequestsImpl.Add(requestTypeType);
    }

    private IServiceProvider ServiceProvider { get; }
    
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        // await using var scope = ServiceProvider.CreateAsyncScope();
        foreach (var requestImpl in RegisteredRequestsImpl)
        {
            if (requestImpl.IsAssignableTo(request.GetType()))
            {
                var handler =
                    ServiceProvider.GetService(
                        typeof(IRequestHandler<,>).MakeGenericType(requestImpl, typeof(TResponse)));
                if (handler == null)
                    continue;
                var func = Expression.Lambda<Func<Task<TResponse>>>(Expression.Call(Expression.Constant(handler), handler.GetType().GetMethod("Handle", new []{requestImpl, typeof(CancellationToken)})!,
                    Expression.Constant(request), Expression.Constant(cancellationToken))).Compile();
                return await func();
            }
        }
        
        throw new NotSupportedException(
                $"Request handler for type {typeof(IRequestHandler<IRequest<TResponse>, TResponse>)} was not registered.");
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