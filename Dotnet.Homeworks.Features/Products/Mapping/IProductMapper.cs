using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Mapster;

namespace Dotnet.Homeworks.Features.Products.Mapping;

[Mapper]
public interface IProductMapper
{
    GetProductsDto Map(IEnumerable<Product> products);
}