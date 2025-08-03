namespace Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public int SubscriptionId { get; private set; }
    public Subscription? Subscription { get; private set; } = null!;

    // EF ctor
    private User() { }

    public User(string name, string email, Subscription subscription)
    {
        Name = name;
        Email = email;
        Subscription = subscription;
        SubscriptionId = subscription.Id;
    }

    public void ChangeEmail(string newEmail)
    {
        Email = newEmail;
    }
    
    public void ChangeName(string newName)
    {
        Email = newName;
    }
}