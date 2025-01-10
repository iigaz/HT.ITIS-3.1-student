using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderValidator: AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        RuleFor(command => command.Id).MustAsync(async (orderId, token) =>
            await orderRepository.GetOrderByGuidAsync(orderId, token) != null);
        RuleFor(command => command.ProductsIds).NotEmpty().ForEach(product => product.MustAsync(async (productId, token) =>
        {
            var prod = await productRepository.GetProductByIdAsync(productId, token);
            return prod != null;
        }));
    }
}