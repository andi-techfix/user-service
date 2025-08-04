using Application.Commands.DeleteUserCommand;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Tests.Commands;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _repoMock = new();
    private static Email ValidEmail => Email.Create("bob@example.com").Value;

    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _handler = new DeleteUserCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_RemoveUserAndSave_When_UserExists()
    {
        // Arrange
        var subscription = Subscription.Create(SubscriptionType.Super, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        var user = new User("Bob", ValidEmail, subscription.Value);

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var cmd = new DeleteUserCommand(1);

        // Act
        await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.Remove(user), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public void Handle_Should_ThrowKeyNotFoundException_When_UserNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User)null!);
        var cmd = new DeleteUserCommand(99);

        // Act
        var act = () => _handler.Handle(cmd, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User not found");
    }
}