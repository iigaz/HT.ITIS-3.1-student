using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Mapster;

namespace Dotnet.Homeworks.Features.Products.Mapping;

public class RegisterProductMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, GetProductDto>()
            .Map(dto => dto.Guid, product => product.Id)
            .RequireDestinationMemberSource(true);
        config.NewConfig<IEnumerable<Product>, GetProductsDto>()
            .Map(dto => dto.Products, product => product)
            .RequireDestinationMemberSource(true);
    }
}