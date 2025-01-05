using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    public ProductRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Products.ToArrayAsync(cancellationToken: cancellationToken);
    }

    public Task DeleteProductByGuidAsync(Guid id, CancellationToken cancellationToken)
    {
        DbContext.Products.Remove(new Product { Id = id });
        return Task.CompletedTask;
    }

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        DbContext.Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        var prod = await DbContext.Products.AddAsync(product, cancellationToken);
        return prod.Entity.Id;
    }
}
