using Domain.Enums;

namespace Domain.Entities;

public class Subscription
{
    public int Id { get; private set; }
    public SubscriptionType Type { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    // EF ctor
    private Subscription() { }

    public Subscription(SubscriptionType type, DateTime start, DateTime end)
    {
        Type = type;
        StartDate = start;
        EndDate = end;
    }
}