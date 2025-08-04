using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.Commands.CreateUserCommand;

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateUserCommand cmd, CancellationToken cancellationToken)
    {
        var subscriptionResult = Subscription.Create(cmd.SubscriptionType, cmd.SubscriptionStart, cmd.SubscriptionEnd);
        var emailResult = Email.Create(cmd.Email);

        var result = Result.Merge(subscriptionResult, emailResult);
        
        if (result.IsFailed) return Result.Fail(result.Errors);
        
        var user = new User(
            cmd.Name,
            emailResult.Value,
            subscriptionResult.Value
        );
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();
        return user.Id;
    }
}