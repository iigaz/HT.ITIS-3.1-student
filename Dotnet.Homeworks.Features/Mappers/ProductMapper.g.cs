using System.Collections.Generic;
using System.Linq;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Mapping;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;

namespace Dotnet.Homeworks.Features.Products.Mapping
{
    public partial class ProductMapper : IProductMapper
    {
        public GetProductsDto Map(IEnumerable<Product> p1)
        {
            return p1 == null ? null : new GetProductsDto(p1 == null ? null : p1.Select<Product, GetProductDto>(funcMain1));
        }
        
        private GetProductDto funcMain1(Product p2)
        {
            return p2 == null ? null : new GetProductDto(p2.Id, p2.Name);
        }
    }
}