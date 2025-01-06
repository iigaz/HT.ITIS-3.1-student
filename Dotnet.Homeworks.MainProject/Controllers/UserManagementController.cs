using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Dto;
using Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Commands.DeleteUser;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Dotnet.Homeworks.Mediator;
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
            return Ok();
        return BadRequest();
    }

    [HttpGet("profile/{guid:guid}")]
    public async Task<IActionResult> GetProfile(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUserQuery(guid), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest();
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllUsersQuery(), cancellationToken);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest();
    }

    [HttpDelete("profile/{guid:guid}")]
    public async Task<IActionResult> DeleteProfile(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteUserCommand(guid), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest();
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(User user, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateUserCommand(user), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest();
    }

    [HttpDelete("user/{guid:guid}")]
    public async Task<IActionResult> DeleteUser(Guid guid, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteUserByAdminCommand(guid), cancellationToken);
        if (result.IsSuccess)
            return Ok();
        return BadRequest();
    }
}