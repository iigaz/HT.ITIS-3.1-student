using Dotnet.Homeworks.Domain.Abstractions.Repositories;

namespace Dotnet.Homeworks.Infrastructure.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken token);
    IProductRepository ProductRepository { get; }
}