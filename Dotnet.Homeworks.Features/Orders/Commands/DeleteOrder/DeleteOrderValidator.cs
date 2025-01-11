using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderValidator: AbstractValidator<DeleteOrderByGuidCommand>
{
    public DeleteOrderValidator(IOrderRepository orderRepository)
    {
        RuleFor(command => command.Id)
            .MustAsync(async (id, token) => await orderRepository.GetOrderByGuidAsync(id, token) != null);
    }
}