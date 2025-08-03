using MediatR;

namespace Application.Commands.DeleteUserCommand;

public record DeleteUserCommand(int Id) : IRequest;