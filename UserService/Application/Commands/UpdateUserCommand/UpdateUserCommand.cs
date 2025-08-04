using FluentResults;
using MediatR;

namespace Application.Commands.UpdateUserCommand;

public record UpdateUserCommand(
    int Id,
    string Name,
    string Email
) : IRequest<Result>;