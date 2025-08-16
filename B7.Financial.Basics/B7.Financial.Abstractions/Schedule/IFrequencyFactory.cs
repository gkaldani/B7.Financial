namespace B7.Financial.Abstractions.Schedule;

/// <summary>
/// Represents a factory for creating frequency instances.
/// </summary>
public interface IFrequencyFactory : INamedFactory<Frequency>
{
    /// <summary>
    /// Retrieves the names of all available predefined frequency instances.
    /// </summary>
    /// <returns> A collection of predefined frequency names. </returns>
    public IEnumerable<Name> FrequencyNames();

    /// <summary>
    /// Creates a <see cref="Frequency"/> instance representing a frequency based on the specified number of days.
    /// </summary>
    /// <param name="days">The number of days to represent as a frequency. Must be a positive integer.</param>
    /// <returns>A <see cref="Frequency"/> instance corresponding to the specified number of days.</returns>
    public static abstract Frequency OfDays(int days);

    /// <summary>
    /// Creates a <see cref="Frequency"/> instance representing a frequency based on the specified number of weeks.
    /// </summary>
    /// <param name="weeks">The number of weeks to represent as a frequency. Must be a positive integer.</param>
    /// <returns>A <see cref="Frequency"/> instance corresponding to the specified number of weeks.</returns>
    public static abstract Frequency OfWeeks(int weeks);

    /// <summary>
    /// Creates a <see cref="Frequency"/> instance representing a recurrence interval in months.
    /// </summary>
    /// <param name="months">The number of months for the recurrence interval. Must be greater than zero.</param>
    /// <returns>A <see cref="Frequency"/> instance configured with the specified monthly interval.</returns>
    public static abstract Frequency OfMonths(int months);

    /// <summary>
    /// Creates a <see cref="Frequency"/> instance representing a recurrence interval in years.
    /// </summary>
    /// <param name="years">The number of years for the recurrence interval. Must be greater than zero.</param>
    /// <returns>A <see cref="Frequency"/> instance configured with the specified yearly interval.</returns>
    public static abstract Frequency OfYears(int years);
}