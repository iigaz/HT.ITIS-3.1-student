using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler: ICommandHandler<UpdateProductCommand>
{
    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private IUnitOfWork UnitOfWork { get; }
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        await UnitOfWork.ProductRepository.UpdateProductAsync(new Product { Id = request.Guid, Name = request.Name },
            cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}