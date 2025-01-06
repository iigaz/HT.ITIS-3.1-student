using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Dotnet.Homeworks.Mediator;

public class Mediator : IMediator
{
    private ConcurrentDictionary<Type, LambdaExpression> RequestToHandler { get; } = new();

    internal void AddHandler(Type requestType, object handler)
    {
        var request = Expression.Parameter(requestType);
        var cancellationToken = Expression.Parameter(typeof(CancellationToken));
        if (handler.GetType().GenericTypeArguments.Length == 0 ||
            handler.GetType().GenericTypeArguments[0] != requestType)
            throw new Exception($"Wrong requestType. Expected: {handler.GetType().GenericTypeArguments[0]}, got {requestType}");
        var method = handler.GetType().GetMethod("Handle", BindingFlags.Default, new [] {requestType, typeof(CancellationToken)});
        if (method == null)
            throw new Exception("Something went wrong.");
        var call = Expression.Call(Expression.Constant(handler), method, request, cancellationToken);
        var lambda = Expression.Lambda(call, false, request, cancellationToken);
        RequestToHandler[requestType] = lambda;
    }

    private Func<TArg, CancellationToken, TResp> SendHelper<TArg, TResp>(Type? requestType)
    {
        if (requestType == null)
            throw new NotSupportedException(
                $"Request handler for type {typeof(TArg)} was not registered.");

        if (RequestToHandler[requestType].Compile() is not Func<TArg, CancellationToken, TResp> func)
            throw new UnreachableException("How Did We Get Here?");
        return func;
    }
    
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = RequestToHandler.Keys.FirstOrDefault(req =>
            req.GenericTypeArguments.Length == 1 && req.GenericTypeArguments[0] == typeof(TResponse));
        var func = SendHelper<IRequest<TResponse>, Task<TResponse>>(requestType);
        return func(request, cancellationToken);
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        var requestType = RequestToHandler.Keys.FirstOrDefault(req => req == typeof(TRequest));
        var func = SendHelper<TRequest, Task>(requestType);
        return func(request, cancellationToken);
    }

    public Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        var requestType = RequestToHandler.Keys.FirstOrDefault(req => req == request.GetType());
        return requestType == null
            ? Task.FromResult(null as object)
            : (Task<dynamic?>)RequestToHandler[requestType].Compile().DynamicInvoke(request, cancellationToken);
    }
}