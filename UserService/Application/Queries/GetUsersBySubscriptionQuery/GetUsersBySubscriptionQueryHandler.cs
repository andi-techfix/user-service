using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.GetUsersBySubscriptionQuery;

public class GetUsersBySubscriptionTypeQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsersBySubscriptionTypeQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetUsersBySubscriptionTypeQuery request,
        CancellationToken cancellationToken)
    {
        return await userRepository.GetBySubscriptionTypeAsync(request.SubscriptionType);
    }
}