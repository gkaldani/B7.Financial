namespace B7.Financial.Abstractions.Date;

/// <summary>
/// Represents a method that adjusts a <see cref="DateOnly"/> value based on custom logic.
/// </summary>
/// <param name="date">The <see cref="DateOnly"/> value to be adjusted.</param>
/// <returns>A new <see cref="DateOnly"/> value that represents the adjusted date.</returns>
public delegate DateOnly DateAdjuster(DateOnly date);
