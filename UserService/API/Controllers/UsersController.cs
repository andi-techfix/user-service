using Application.Commands.CreateUserCommand;
using Application.Commands.DeleteUserCommand;
using Application.Commands.UpdateUserCommand;
using Application.Dtos;
using Application.Queries.GetAllUsersQuery;
using Application.Queries.GetUserByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public Task<IEnumerable<UserDto>> GetAll()
        => mediator.Send(new GetAllUsersQuery());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var dto = await mediator.Send(new GetUserByIdQuery(id));
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand cmd)
    {
        var id = await mediator.Send(cmd);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand cmd)
    {
        if (id != cmd.Id) return BadRequest();
        await mediator.Send(cmd);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }
}