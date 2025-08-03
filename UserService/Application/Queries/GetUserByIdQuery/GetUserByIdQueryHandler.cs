using Application.Dtos;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.GetUserByIdQuery;

public class GetUserByIdQueryHandler(IUserRepository repo) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery q, CancellationToken ct)
    {
        var u = await repo.GetByIdAsync(q.Id);
        if (u is null) return null;
        return new UserDto {
            Id                = u.Id,
            Name              = u.Name,
            Email             = u.Email,
            SubscriptionType  = u.Subscription.Type,
            SubscriptionStart = u.Subscription.StartDate,
            SubscriptionEnd   = u.Subscription.EndDate
        };
    }
}