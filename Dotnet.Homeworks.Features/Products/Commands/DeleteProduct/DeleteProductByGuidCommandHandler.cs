using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;

internal sealed class DeleteProductByGuidCommandHandler
    : ICommandHandler<DeleteProductByGuidCommand>
{
    public DeleteProductByGuidCommandHandler(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    private IUnitOfWork UnitOfWork { get; }

    public async Task<Result> Handle(
        DeleteProductByGuidCommand request,
        CancellationToken cancellationToken
    )
    {
        await UnitOfWork.ProductRepository.DeleteProductByGuidAsync(
            request.Guid,
            cancellationToken
        );
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return new Result(true);
    }
}
