using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Abstractions.Date;

/// <summary>
/// A convention defining how a period is added to a date.
/// <para>
/// The purpose of this convention is to define how to handle the addition of a period. <br/>
/// The default implementations include two different end-of-month rules. <br/>
/// The convention is generally only applicable for month-based periods.
/// </para>
/// <para>
/// Implementations of this interface are provided in <see cref="IPeriodAdditionConventionFactory"/>
/// </para>
/// </summary>
public interface IPeriodAdditionConvention : INamed
{
    /// <summary>
    /// Adjusts the base date, adding the period and applying the convention rule.
    /// <para>
    /// The adjustment occurs in two steps. <br/>
    /// First, the period is added to the based date to create the end date. <br/>
    /// Second, the end date is adjusted by the convention rules. <br/>
    /// </para>
    /// </summary>
    /// <param name="date">The date to add to</param>
    /// <param name="period">The period to add</param>
    /// <param name="calendar">The holiday calendar to use</param>
    /// <returns>The adjusted date</returns>
    DateOnly Adjust(DateOnly date, Period period, IHolidayCalendar calendar);
    
    /// <summary>
    /// Checks whether the convention requires a month-based period.
    /// <para>
    /// A month-based period contains only months and/or years, and not days.
    /// </para>
    /// </summary>
    bool IsMonthBased { get; }
}