using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator(IProductRepository productRepository)
    {
        RuleFor(order => order.ProductsIds).NotEmpty().ForEach(product => product.MustAsync(async (productId, token) =>
        {
            var prod = await productRepository.GetProductByIdAsync(productId, token);
            return prod != null;
        }));
    }
}