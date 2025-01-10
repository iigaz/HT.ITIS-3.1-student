using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

internal sealed class InsertProductCommandHandler: ICommandHandler<InsertProductCommand, InsertProductDto>
{
    public InsertProductCommandHandler(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private IUnitOfWork UnitOfWork { get; }
    
    public async Task<Result<InsertProductDto>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.ProductRepository.InsertProductAsync(new Product() { Id = Guid.NewGuid(), Name = request.Name },
            cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result<InsertProductDto>(new InsertProductDto(result), true);
    }
}