using Application.Dtos;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.GetAllUsersQuery;

public class GetAllUsersQueryHandler(IUserRepository repo) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery _, CancellationToken ct)
    {
        var users = await repo.GetAllAsync();
        return users.Select(u => new UserDto {
            Id                = u.Id,
            Name              = u.Name,
            Email             = u.Email,
            SubscriptionType  = u.Subscription.Type,
            SubscriptionStart = u.Subscription.StartDate,
            SubscriptionEnd   = u.Subscription.EndDate
        });
    }
}