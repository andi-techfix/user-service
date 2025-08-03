using Domain.Repositories;
using MediatR;

namespace Application.Commands.UpdateUserCommand;

public class UpdateUserCommandHandler(IUserRepository repo) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand cmd, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(cmd.Id);
        if (user is null) throw new KeyNotFoundException("User not found");

        user.ChangeName(newName:cmd.Name);
        user.ChangeEmail(newEmail:cmd.Email);

        repo.Update(user);
        await repo.SaveChangesAsync();
    }
}