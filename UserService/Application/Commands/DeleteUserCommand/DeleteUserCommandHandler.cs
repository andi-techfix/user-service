using Domain.Repositories;
using MediatR;

namespace Application.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler(IUserRepository repo) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand cmd, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(cmd.Id);
        if (user is null) throw new KeyNotFoundException("User not found");

        repo.Remove(user);
        await repo.SaveChangesAsync();
    }
}