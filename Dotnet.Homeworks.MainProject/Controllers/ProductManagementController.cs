using System.Diagnostics;
using System.Diagnostics.Metrics;
using Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class ProductManagementController : ControllerBase
{
    private static readonly Counter<int> GetProductsHitCounter =
        new Meter("Dotnet.Homeworks.Meter").CreateCounter<int>("GetProductsHitCounter");

    private static readonly ActivitySource ActivitySource = new("Dotnet.Homeworks.Source");

    public ProductManagementController(IMediator mediator)
    {
        Mediator = mediator;
    }

    private IMediator Mediator { get; }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity();
        GetProductsHitCounter.Add(1);
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