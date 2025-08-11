using System.Runtime.CompilerServices;
using System.Text;

namespace B7.Financial.Abstractions;

/// <summary>
/// Represents a strongly-typed name.
/// </summary>
/// <remarks>
/// Has implicit conversions to and from <see cref="string"/>.
/// </remarks>
#pragma warning disable CA2231
public readonly struct Name : IEquatable<Name>
#pragma warning restore CA2231
{
    /// <summary>
    /// Determines the maximum length of a name.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Factory method to create a new instance of <see cref="Name"/> with the specified value.
    /// </summary>
    /// <param name="value">The value of the name.</param>
    /// <returns>A new instance of <see cref="Name"/>.</returns>
    public static Name Create(string value) => new(value);

    /// <summary>
    /// The string representation of the name.
    /// </summary>
    public string Value => _value ?? throw new InvalidOperationException("Name is not initialized.");

    // Backing field so default(Name) is safe.
    private readonly string? _value;

    /// <summary>
    /// Constructs a new instance of <see cref="Name"/> with the specified value.
    /// </summary>
    /// <param name="value">The value of the name.</param>
    /// <exception cref="ArgumentException">Thrown when the name is null, whitespace</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the name is too long.</exception>
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(value));

        _value = value.Trim().Normalize(NormalizationForm.FormC);

        if (_value.Length > MaxLength)
            throw new ArgumentOutOfRangeException(nameof(value), _value.Length, $"Name cannot be longer than {MaxLength} characters.");
    }

    /// <summary>
    /// Creates a new readonly span over the <see cref="Name"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<char> AsSpan() => Value.AsSpan();

    /// <summary>
    /// Overrides the ToString method to return the name value.
    /// </summary>
    /// <returns>The name value.</returns>
    public override string ToString() => Value;

    /// <inheritdoc/>
    public bool Equals(Name other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Name { _value: not null } other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    /// Implicit conversion from <see cref="Name"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="name"> The name instance to convert. </param>
    public static implicit operator string(Name name) => name.Value;

    /// <summary>
    /// Implicit conversion from <see cref="string"/> to <see cref="Name"/>.
    /// </summary>
    /// <param name="value"> The string value to convert. </param>
    public static implicit operator Name(string value) => new (value);
}