using Dotnet.Homeworks.Features.Products.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsDto>
{
    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IProductMapper productMapper)
    {
        UnitOfWork = unitOfWork;
        ProductMapper = productMapper;
    }

    private IUnitOfWork UnitOfWork { get; }
    private IProductMapper ProductMapper { get; }

    public async Task<Result<GetProductsDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.ProductRepository.GetAllProductsAsync(cancellationToken);
        return new Result<GetProductsDto>(ProductMapper.Map(result), true);
    }
}