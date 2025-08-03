using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Commands.CreateUserCommand;

public record CreateUserCommand(
    string Name,
    string Email,
    SubscriptionType SubscriptionType,
    DateTime SubscriptionStart,
    DateTime SubscriptionEnd
) : IRequest<int>;