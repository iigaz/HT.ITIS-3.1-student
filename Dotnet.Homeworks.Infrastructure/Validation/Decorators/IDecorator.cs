using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public interface IDecorator<in TRequest, TResponse>
{
    public Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}