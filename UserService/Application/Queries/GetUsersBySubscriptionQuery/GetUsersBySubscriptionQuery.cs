using Domain.Entities;
using MediatR;

namespace Application.Queries.GetUsersBySubscriptionQuery;

public record GetUsersBySubscriptionTypeQuery(string SubscriptionType) : IRequest<IEnumerable<User>>;