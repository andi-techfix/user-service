using Domain.Entities;
using Domain.Enums;

namespace Application.Dtos;

public class UserDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public SubscriptionType SubscriptionType { get; init; }
    public DateTime SubscriptionStart { get; init; }
    public DateTime SubscriptionEnd { get; init; }
}