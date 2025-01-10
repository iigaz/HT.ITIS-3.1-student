using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

public class ServiceMediator : IMediator
{
    private ConcurrentBag<Type> RegisteredRequestsImpl { get; } = new();

    public ServiceMediator(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    internal void AddRequestImpl(Type requestTypeType)
    {
        RegisteredRequestsImpl.Add(requestTypeType);
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        foreach (var requestImpl in RegisteredRequestsImpl)
            if (requestImpl.IsAssignableTo(request.GetType()))
            {
                var handler =
                    scope.ServiceProvider.GetService(
                        typeof(IRequestHandler<,>).MakeGenericType(requestImpl, typeof(TResponse)));
                if (handler == null)
                    continue;
                var func = Expression.Lambda<RequestHandlerDelegate<TResponse>>(Expression.Call(
                    Expression.Constant(handler),
                    handler.GetType().GetMethod("Handle", new[] { requestImpl, typeof(CancellationToken) })!,
                    Expression.Constant(request), Expression.Constant(cancellationToken))).Compile();

                var pipeline = func;
                var pipelines = scope.ServiceProvider.GetServices(
                        typeof(IPipelineBehavior<,>).MakeGenericType(requestImpl, typeof(TResponse)))
                    .ToArray();
                if (pipelines.Length > 0)
                    foreach (var pipe in pipelines.Reverse())
                    {
                        if (pipe == null)
                            continue;
                        pipeline = Expression.Lambda<RequestHandlerDelegate<TResponse>>(Expression.Call(
                            Expression.Constant(pipe),
                            pipe.GetType().GetMethod("Handle",
                                new[]
                                {
                                    requestImpl, typeof(RequestHandlerDelegate<TResponse>), typeof(CancellationToken)
                                })!, Expression.Constant(request), Expression.Constant(pipeline),
                            Expression.Constant(cancellationToken))).Compile();
                    }

                return await pipeline();
            }

        throw new NotSupportedException(
            $"Request handler for type {typeof(IRequestHandler<IRequest<TResponse>, TResponse>)} was not registered.");
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
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
        var handler = scope.ServiceProvider.GetService(typeof(IRequestHandler<>).MakeGenericType(request.GetType()));
        return handler == null ? null : await handler.Handle(request, cancellationToken);
    }
}