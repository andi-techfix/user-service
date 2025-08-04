using System.Reflection;
using Application.Queries.GetAllUsersQuery;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Tests.Queries;

public class GetAllUsersQueryHandlerTests
{
    private static readonly DateTime ValidStart = DateTime.UtcNow;
    private static readonly DateTime ValidEnd = ValidStart.AddDays(7);


    private const SubscriptionType FreeType = SubscriptionType.Free;
    private const SubscriptionType SuperType = SubscriptionType.Super;

    private static readonly User User1 = CreateUser(
        id: 1,
        name: "Alice",
        email: "alice@ex.com",
        type: FreeType
    );

    private static readonly User User2 = CreateUser(
        id: 2,
        name: "Bob",
        email: "bob@ex.com",
        type: SuperType
    );

    private static readonly List<User> AllUsers = [User1, User2];

    private static User CreateUser(int id, string name, string email, SubscriptionType type)
    {
        var sub = Subscription.Create(type, ValidStart, ValidEnd);
        var u = new User(name, Email.Create(email).Value, sub.Value);
        var prop = typeof(User)
            .GetProperty(nameof(User.Id), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
        prop.SetValue(u, id);
        return u;
    }

    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly GetAllUsersQueryHandler _handler;

    public GetAllUsersQueryHandlerTests()
    {
        _handler = new GetAllUsersQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_MapAllDomainUsersToDtos()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(AllUsers);

        // Act
        var dtos = await _handler.Handle(new GetAllUsersQuery(), CancellationToken.None);

        // Assert
        var list = dtos.ToList();
        list.Should().HaveCount(2);

        list.Should().ContainSingle(d =>
            d.Id == 1 &&
            d.Name == "Alice" &&
            d.Email == "alice@ex.com" &&
            d.SubscriptionType == FreeType &&
            d.SubscriptionStart == ValidStart &&
            d.SubscriptionEnd == ValidEnd
        );

        list.Should().ContainSingle(d =>
            d.Id == 2 &&
            d.Name == "Bob" &&
            d.Email == "bob@ex.com" &&
            d.SubscriptionType == SuperType &&
            d.SubscriptionStart == ValidStart &&
            d.SubscriptionEnd == ValidEnd
        );
    }
}