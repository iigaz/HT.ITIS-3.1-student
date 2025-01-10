using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;
using Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class OrderManagementController : ControllerBase
{
    public OrderManagementController(IMediator mediator)
    {
        Mediator = mediator;
    }

    private IMediator Mediator { get; }

    [HttpGet("orders")]
    public async Task<IActionResult> GetUserOrdersAsync(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetOrdersQuery(), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpGet("order/{id:guid}")]
    public async Task<IActionResult> GetUserOrdersAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetOrderQuery(id), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost("order")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] IEnumerable<Guid> productsIds,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateOrderCommand(productsIds), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPut("order/{id:guid}")]
    public async Task<IActionResult> UpdateOrderAsync(Guid id, [FromBody] IEnumerable<Guid> productsIds,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateOrderCommand(id, productsIds), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }

    [HttpDelete("order/{id:guid}")]
    public async Task<IActionResult> DeleteOrderAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteOrderByGuidCommand(id), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}