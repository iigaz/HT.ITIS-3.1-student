using Dotnet.Homeworks.Shared.Dto;
using Dotnet.Homeworks.Mediator;

namespace Dotnet.Homeworks.Infrastructure.Cqrs.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery: IQuery<TResponse>
{
}