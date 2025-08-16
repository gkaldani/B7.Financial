namespace B7.Financial.Abstractions.Schedule;

/// <summary>
/// Represents a factory for creating roll convention instances.
/// </summary>
public interface IRollConventionFactory : INamedFactory<IRollConvention>
{
    /// <summary>
    /// Retrieves the names of all available predefined <see cref="IRollConvention"/> instances.
    /// </summary>
    /// <returns> A collection of predefined Roll Convention names. </returns>
    public IEnumerable<Name> RollConventionNames();

    /// <summary>
    /// Obtains an instance from the day-of-month.
    /// <para>
    /// This convention will adjust the input date to the specified day-of-month. <br/>
    /// The year and month of the result date will be the same as the input date. <br/>
    /// It is intended for use with periods that are a multiple of months.
    /// </para>
    /// <para>
    /// If the month being adjusted has a length less than the requested day-of-month <br/>
    /// then the last valid day-of-month will be chosen. As such, passing 31 to this <br/>
    /// method is equivalent to selecting the end-of-month convention.
    /// </para>
    /// </summary>
    /// <param name="dayOfMonth">The day-of-month, form 1 to 31</param>
    /// <returns>The <see cref="IRollConvention"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the day-of-month is not between 1 and 31.</exception>
    public static abstract IRollConvention OfDayOfMonth(byte dayOfMonth);

    /// <summary>
    /// Obtains an instance from the day-of-week.
    /// <para>
    /// This convention will adjust the input date to the specified day-of-week. <br/>
    /// It is intended for use with <see cref="Frequency.Period"/> that are a multiple of weeks.
    /// </para>
    /// <para>
    /// In <see cref="IRollConvention.Adjust"/>, if the input date is not the required day-of-week, <br/>
    /// then the next occurrence of the day-of-week is selected, up to 6 days later.
    /// </para>
    /// <para>
    /// In <see cref="IRollConvention.Next"/>, the day-of-week is selected after the frequency is added. <br/>
    /// If the calculated date is not the required day-of-week, then the next occurrence <br/>
    /// of the day-of-week is selected, up to 6 days later.
    /// </para>
    /// <para>
    /// In <see cref="IRollConvention.Previous"/>, the day-of-week is selected after the frequency is subtracted. <br/>
    /// If the calculated date is not the required day-of-week, then the previous occurrence <br/>
    /// of the day-of-week is selected, up to 6 days earlier.
    /// </para>
    /// </summary>
    /// <param name="dayOfWeek">The day-of-week.</param>
    /// <returns>The <see cref="IRollConvention"/> instance.</returns>
    public static abstract IRollConvention OfDayOfWeek(DayOfWeek dayOfWeek);
}