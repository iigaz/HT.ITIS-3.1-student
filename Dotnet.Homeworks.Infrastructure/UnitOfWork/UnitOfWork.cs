using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;

namespace Dotnet.Homeworks.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
        ProductRepository = new ProductRepository(appDbContext);
    }

    private AppDbContext AppDbContext { get; }

    public IProductRepository ProductRepository { get; }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await AppDbContext.SaveChangesAsync(token);
    }
}
