using Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class ProductManagementController : ControllerBase
{
    public ProductManagementController(IMediator mediator)
    {
        Mediator = mediator;
    }

    private IMediator Mediator { get; }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductsQuery(), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpPost("product")]
    public async Task<IActionResult> InsertProduct(string name, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new InsertProductCommand(name), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteProductByGuidCommand(guid), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }

    [HttpPut("product")]
    public async Task<IActionResult> UpdateProduct(
        Guid guid,
        string name,
        CancellationToken cancellationToken
    )
    {
        var result = await Mediator.Send(new UpdateProductCommand(guid, name), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}
