using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler: IQueryHandler<GetProductsQuery, GetProductsDto>
{
    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private IUnitOfWork UnitOfWork { get; }
    public async Task<Result<GetProductsDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.ProductRepository.GetAllProductsAsync(cancellationToken);
        return new Result<GetProductsDto>(
            new GetProductsDto(result.Select(prod => new GetProductDto(prod.Id, prod.Name))), true);
    }
}