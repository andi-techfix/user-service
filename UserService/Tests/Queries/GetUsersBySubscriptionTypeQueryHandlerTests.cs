using System.Reflection;
using Application.Queries.GetUsersBySubscriptionQuery;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Tests.Queries;

public class GetUsersBySubscriptionTypeQueryHandlerTests
{
    private static readonly DateTime ValidStart = DateTime.UtcNow;
    private static readonly DateTime ValidEnd = ValidStart.AddDays(3);

    private const SubscriptionType SuperType = SubscriptionType.Super;

    private static readonly User UserDan = CreateUser(1, "Dan", "dan@ex.com", SuperType);
    private static readonly User UserEli = CreateUser(2, "Eli", "eli@ex.com", SuperType);

    private static User CreateUser(int id, string name, string email, SubscriptionType type)
    {
        var subscriptionResult = Subscription.Create(type, ValidStart, ValidEnd);
        var user = new User(name, Email.Create(email).Value, subscriptionResult.Value);
        var prop = typeof(User)
            .GetProperty(nameof(User.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
        prop.SetValue(user, id);
        return user;
    }

    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly GetUsersBySubscriptionTypeQueryHandler _handler;

    public GetUsersBySubscriptionTypeQueryHandlerTests()
    {
        _handler = new GetUsersBySubscriptionTypeQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllUsers_WithGivenSubscriptionType()
    {
        // Arrange
        var list = new List<User> { UserDan, UserEli };
        _repoMock
            .Setup(r => r.GetBySubscriptionTypeAsync(SuperType.ToString()))
            .ReturnsAsync(list);

        // Act
        var result = await _handler.Handle(
            new GetUsersBySubscriptionTypeQuery(SuperType.ToString()),
            CancellationToken.None
        );

        // Assert
        result.Should().BeSameAs(list);
    }
}