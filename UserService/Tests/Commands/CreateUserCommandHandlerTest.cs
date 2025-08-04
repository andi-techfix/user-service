// File: Tests.Commands.CreateUserCommandHandlerTests.cs

using System.Reflection;
using Application.Commands.CreateUserCommand;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using FluentAssertions;
using FluentResults;
using Moq;

namespace Tests.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_AddUserAndReturnAssignedId()
    {
        // Arrange
        var start = DateTime.UtcNow;
        var end = start.AddDays(30);
        var cmd = new CreateUserCommand(
            Name: "Alice",
            Email: "alice@example.com",
            SubscriptionType: SubscriptionType.Super,
            SubscriptionStart: start,
            SubscriptionEnd: end
        );

        // Simulate EF assigning an Id inside AddAsync
        _repoMock
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u =>
            {
                var prop = typeof(User)
                    .GetProperty(nameof(User.Id),
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
                prop.SetValue(u, 42);
            })
            .Returns(Task.CompletedTask);

        _repoMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        Result<int> result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);

        _repoMock.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Name == "Alice" &&
            u.Email.Value == "alice@example.com" &&
            u.Subscription.Type == SubscriptionType.Super &&
            u.Subscription.StartDate == start &&
            u.Subscription.EndDate == end
        )), Times.Once);

        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}