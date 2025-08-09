using static B7.Financial.Abstractions.Date.IDayCount;

namespace B7.Financial.Abstractions.Date;

/// <summary>
/// A convention defining how to calculate fractions of a year.
/// </summary>
/// <remarks>
/// The purpose of this convention is to define how to convert dates into numeric year fractions. <br/>
/// This is of use when calculating accrued interest over time.
/// </remarks>
public abstract class DayCount : IDayCount
{
    public static string Name => throw new NotImplementedException("DayCount");

    /// <summary>
    /// Unique name of the day count convention.
    /// </summary>
    public abstract string GetName();

    /// <summary>
    /// Gets the year fraction between the specified dates. <br/>
    /// Given two dates, this method returns the fraction of a year between these <br/>
    /// dates according to the convention. The dates must be in order.
    /// </summary>
    /// <param name="firstDate">The first date</param>
    /// <param name="secondDate">The second date, on or after the <see cref="firstDate"/></param>
    /// <returns>The year fraction, zero or greater</returns>
    public decimal YearFraction(DateOnly firstDate, DateOnly secondDate) =>
        YearFraction(firstDate, secondDate, null);

    public abstract decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IScheduleInfo? scheduleInfo);

    public abstract int Days(DateOnly firstDate, DateOnly secondDate);
}