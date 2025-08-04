using System.Reflection;
using Application.Queries.GetUserByIdQuery;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Tests.Queries;

public class GetUserByIdQueryHandlerTests
{
    private static readonly DateTime ValidStart = DateTime.UtcNow;
    private static readonly DateTime ValidEnd = ValidStart.AddDays(14);
    private const SubscriptionType TrialType = SubscriptionType.Trial;

    private static readonly User ExistingUser = CreateUser(
        id: 5,
        name: "Carol",
        email: "carol@ex.com",
        type: TrialType
    );

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
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _handler = new GetUserByIdQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_UserNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(new GetUserByIdQuery(99), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_MapToDto_When_UserExists()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByIdAsync(ExistingUser.Id))
            .ReturnsAsync(ExistingUser);

        // Act
        var dto = await _handler.Handle(new GetUserByIdQuery(ExistingUser.Id), CancellationToken.None);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(ExistingUser.Id);
        dto.Name.Should().Be("Carol");
        dto.Email.Should().Be("carol@ex.com");
        dto.SubscriptionType.Should().Be(TrialType);
        dto.SubscriptionStart.Should().Be(ValidStart);
        dto.SubscriptionEnd.Should().Be(ValidEnd);
    }
}