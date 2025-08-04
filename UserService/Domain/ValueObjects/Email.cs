using System.Text.RegularExpressions;
using FluentResults;

namespace Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail<Email>("Email cannot be empty.");

        return !EmailRegex.IsMatch(email)
            ? Result.Fail<Email>($"'{email}' is not a valid email address.")
            : Result.Ok(new Email(email));
    }

    public override bool Equals(object? obj) => Equals(obj as Email);

    public bool Equals(Email? other) => other is not null
                                        && Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(Email e) => e.Value;
}