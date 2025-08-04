using Application.Commands.CreateUserCommand;
using Application.Commands.DeleteUserCommand;
using Application.Commands.UpdateUserCommand;
using Application.Dtos;
using Application.Queries.GetAllUsersQuery;
using Application.Queries.GetUserByIdQuery;
using Application.Queries.GetUsersBySubscriptionQuery;
using Domain.Entities;
using FluentResults;
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

    [HttpGet("bySubscription/{subscriptionType}")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersBySubscriptionType(string subscriptionType)
    {
        var users = await mediator.Send(new GetUsersBySubscriptionTypeQuery(subscriptionType));
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand cmd)
    {
        var creationIdResult = await mediator.Send(cmd);
        if(creationIdResult.IsFailed)
        {
            return BadRequest(creationIdResult.Errors);
        }
        return Ok(creationIdResult.Value);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand cmd)
    {
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