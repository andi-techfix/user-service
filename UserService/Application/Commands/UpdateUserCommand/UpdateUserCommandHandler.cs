using Domain.Repositories;
using Domain.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.Commands.UpdateUserCommand;

public class UpdateUserCommandHandler(IUserRepository repo) : IRequestHandler<UpdateUserCommand,  Result>
{
    public async Task<Result> Handle(UpdateUserCommand cmd, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(cmd.Id);
        if (user is null) throw new KeyNotFoundException("User not found");

        var emailResult = Email.Create(cmd.Email);
        
        if (emailResult.IsFailed)
            return Result.Fail(emailResult.Errors);
        
        user.ChangeName(newName:cmd.Name);
        user.ChangeEmail(emailResult.Value);

        repo.Update(user);
        await repo.SaveChangesAsync();
        
        return Result.Ok();
    }
}