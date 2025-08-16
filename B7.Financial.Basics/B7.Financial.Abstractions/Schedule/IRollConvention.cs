namespace B7.Financial.Abstractions.Schedule;

/// <summary>
/// A convention defining how to roll dates.
/// <para>
/// A <see cref="Frequency"/> defines the periodicity of the roll. <br/>
/// When applying the frequency, the roll convention is used to fine tune the dates. <br/>
/// This might involve selecting the last day of the month, or the third Wednesday.
/// </para>
/// <para>
/// To get the next date in the schedule, take the base date and the <br/>
/// periodic <see cref="Frequency"/> and apply the roll convention. <br/>
/// the roll convention is applied to produce the next schedule date.
/// </para>
/// </summary>
public interface IRollConvention : INamed
{
    /// <summary>
    /// Adjusts the given date according to the roll convention.
    /// <para>
    /// It is recommended to use <see cref="Next"/> and <see cref="Previous"/> rather than <br/>
    /// directly using this method.
    /// </para>
    /// </summary>
    /// <param name="date">The date to adjust.</param>
    /// <returns>The adjusted date.</returns>
    DateOnly Adjust(DateOnly date);

    /// <summary>
    /// Checks if the date matches the rules of the roll convention. <br/>
    /// See the description of each roll convention to understand the rule applied.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>True if the date matches the roll convention; otherwise, false.</returns>
    public bool Matches(DateOnly date);

    /// <summary>
    /// Calculates the next date based on the given date and periodic frequency. <br/>
    /// This takes the input date, adds the periodic frequency and adjusts the date <br/>
    /// as necessary to match the roll convention rules. <br/>
    /// The result will always be after the input date.
    /// </summary>
    /// <param name="date">The date to adjust.</param>
    /// <param name="periodicFrequency">The frequency of the periodic event.</param>
    /// <returns>The next date based on the roll convention.</returns>
    DateOnly Next(DateOnly date, Frequency periodicFrequency);

    /// <summary>
    /// Calculates the previous date based on the specified starting date and periodic frequency. <br/>
    /// This takes the input date, subtracts the periodic frequency and adjusts the date <br/>
    /// as necessary to match the roll convention rules. <br/>
    /// The result will always be before the input date.
    /// </summary>
    /// <param name="date">The date to adjust.</param>
    /// <param name="periodicFrequency">The frequency of the periodic event.</param>
    /// <returns>The previous date based on the roll convention.</returns>
    DateOnly Previous(DateOnly date, Frequency periodicFrequency);

    /// <summary>
    /// Gets the day-of-month that the roll convention implies.
    /// <para>
    /// This extracts the day-of-month for simple roll conventions. <br/>
    /// The numeric roll conventions will return their day-of-month. <br/>
    /// The 'EOM' convention will return 31. <br/>
    /// All other conventions will return zero (0).
    /// </para>
    /// </summary>
    byte DayOfMonth { get; }
}