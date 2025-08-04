// Domain/Entities/Subscription.cs

using System;
using FluentResults;
using Domain.Enums;

namespace Domain.Entities;

public class Subscription
{
    public int Id { get; private set; }
    public SubscriptionType Type { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private Subscription()
    {
    }

    private Subscription(SubscriptionType type, DateTime start, DateTime end)
    {
        Type = type;
        StartDate = start;
        EndDate = end;
    }
    
    public static Result<Subscription> Create(
        SubscriptionType type,
        DateTime start,
        DateTime end)
    {
        return end <= start
            ? Result.Fail<Subscription>("EndDate must be after StartDate.")
            : Result.Ok(new Subscription(type, start, end));
    }
}