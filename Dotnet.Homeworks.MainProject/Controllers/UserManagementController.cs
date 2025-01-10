using System.Security.Claims;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Dto;
using Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RegisterUserDto = Dotnet.Homeworks.MainProject.Dto.RegisterUserDto;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class UserManagementController : ControllerBase
{
    public UserManagementController(IMediator mediator)
    {
        Mediator = mediator;
    }

    private IMediator Mediator { get; }

    [HttpPost("user")]
    public async Task<IActionResult> CreateUser(RegisterUserDto userDto, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateUserCommand(userDto.Name, userDto.Email), cancellationToken);
        if (result.IsSuccess)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, result.Value!.Guid.ToString()),
                new(ClaimTypes.Role, "User")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }


    [HttpPost("login/{guid:guid}")]
    public async Task<IActionResult> Login(Guid guid, CancellationToken cancellationToken,
        [FromServices] IWebHostEnvironment webHostEnvironment)
    {
        if (!webHostEnvironment.IsDevelopment())
            return NotFound();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, guid.ToString()),
            new(ClaimTypes.Role, "User")
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        return Ok();
    }

    [HttpGet("profile/{guid:guid}")]
    public async Task<IActionResult> GetProfile(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserQuery(guid), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllUsersQuery(), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Error);
    }

    [HttpDelete("profile/{guid:guid}")]
    public async Task<IActionResult> DeleteProfile(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteUserCommand(guid), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(User user, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateUserCommand(user), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }

    [HttpDelete("user/{guid:guid}")]
    public async Task<IActionResult> DeleteUser(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteUserByAdminCommand(guid), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.Error);
    }
}