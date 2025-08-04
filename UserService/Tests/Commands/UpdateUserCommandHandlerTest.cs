using Application.Commands.UpdateUserCommand;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Tests.Commands;

public class UpdateUserCommandHandlerTests
{
    private static Email ValidEmail => Email.Create("carol@example.com").Value;
    private readonly Mock<IUserRepository> _repoMock = new();
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _handler = new UpdateUserCommandHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_UpdateFieldsAndSave_When_UserExists()
    {
        // Arrange
        var subscription = Subscription.Create(SubscriptionType.Free, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        var user = new User("Carol", ValidEmail, subscription.Value);

        _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(user);
        _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var cmd = new UpdateUserCommand(
            Id: 2,
            Name: "Carol New",
            Email: "carol.new@example.com"
        );

        // Act
        await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        user.Name.Should().Be("Carol New");
        user.Email.Value.Should().Be("carol.new@example.com");
        _repoMock.Verify(r => r.Update(user), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public void Handle_Should_ThrowKeyNotFoundException_When_UserNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(123)).ReturnsAsync((User)null!);
        var cmd = new UpdateUserCommand(123, "Name", "email");

        // Act
        var act = () => _handler.Handle(cmd, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User not found");
    }
}