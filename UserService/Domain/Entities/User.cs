using Domain.ValueObjects;

namespace Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public int SubscriptionId { get; private set; }
    public Subscription Subscription { get; private set; } = null!;

    // EF ctor
    private User() { }

    public User(string name, Email email, Subscription subscription)
    {
        Name = name;
        Email = email;
        Subscription = subscription;
        SubscriptionId = subscription.Id;
    }

    public void ChangeEmail(Email newEmail)
    {
        Email = newEmail;
    }
    
    public void ChangeName(string newName)
    {
        Name = newName;
    }
}