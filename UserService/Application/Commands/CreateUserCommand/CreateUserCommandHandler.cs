using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Commands.CreateUserCommand;

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, int>
{
    public async Task<int> Handle(CreateUserCommand cmd, CancellationToken cancellationToken)
    {
        var subscription = new Subscription(cmd.SubscriptionType, cmd.SubscriptionStart, cmd.SubscriptionEnd);
        var user = new User(
            cmd.Name,
            cmd.Email,
            subscription
        );
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();
        return user.Id;
    }
}