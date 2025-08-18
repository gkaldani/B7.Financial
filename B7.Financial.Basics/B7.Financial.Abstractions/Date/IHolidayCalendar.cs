namespace B7.Financial.Abstractions.Date;

/// <summary>
/// A holiday calendar, classifying dates as holidays or business days.
/// <para>
/// Many calculations in finance require knowledge of whether a date is a business day or not. <br/>
/// This interface encapsulates that knowledge, with each day treated as a holiday or a business day. <br/>
/// Weekends are effectively treated as a special kind of holiday.
/// </para>
/// <para>
/// Applications should refer to holidays using <see cref="IHolidayCalendarFactory"/>. <br/>
/// The name must be resolved <see cref="IHolidayCalendarFactory.Of"/> to a <see cref="IHolidayCalendar"/> <br/>
/// before the holiday data methods can be accessed. <br/>
/// </para>
/// </summary>
public interface IHolidayCalendar : INamed, IEquatable<IHolidayCalendar>
{
    /// <summary>
    /// Checks if the specified date is a holiday. <br/>
    /// This is the opposite of <see cref="IsBusinessDay"/>. <br/>
    /// </summary>
    bool IsHoliday(DateOnly date);

    /// <summary>
    /// Checks if the specified date is a business day. <br/>
    /// This is the opposite of <see cref="IsHoliday"/>. <br/>
    /// </summary>
    bool IsBusinessDay(DateOnly date);

    /// <summary>
    /// Returns an adjuster that changes the date.
    /// </summary>
    /// <param name="days">The number of business days to adjust by</param>
    /// <returns></returns>
    DateAdjuster AdjustBy(int days);

    /// <summary>
    /// Shifts the date by a specified number of business days.
    /// <para>
    /// If the number of days is zero, the original date is returned. <br/>
    /// If the number of days is positive, the date is shifted forward. Later business days are chosen. <br/>
    /// If the number of days is negative, the date is shifted backward. Earlier business days are chosen. <br/>
    /// </para>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <param name="days"></param>
    /// <returns></returns>
    DateOnly Shift(DateOnly date, int days);

    /// <summary>
    /// Finds the next business day, always returning a later date.
    /// <para>Given a date, this method returns the next business day.</para>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <returns>The first business day after the input date</returns>
    DateOnly Next(DateOnly date);

    /// <summary>
    /// Finds the next business day, returning the input date if it is a business day.
    /// <para>
    /// Given a date, this method returns a business day. <br/>
    /// If the input date is a business day, it is returned. <br/>
    /// Otherwise, the next business day is returned.
    /// </para>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <returns>The input date if it is a business day, or the next business day</returns>
    DateOnly NextOrSame(DateOnly date);

    /// <summary>
    /// Finds the previous business day, always returning an earlier date.
    /// <para>Given a date, this method returns the previous business day.</para>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <returns>The first business day before the input date</returns>
    DateOnly Previous(DateOnly date);

    /// <summary>
    /// Finds the previous business day, returning the input date if it is a business day.
    /// <para>
    /// Given a date, this method returns a business day. <br/>
    /// If the input date is a business day, it is returned. <br/>
    /// Otherwise, the previous business day is returned.
    /// </para>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <returns>The input date if it is a business day, or the previous business day</returns>
    DateOnly PreviousOrSame(DateOnly date);

    /// <summary>
    /// Finds the next business day within the month, returning the input date if it is a business day, <br/>
    /// or the last business day of the month if the next business day is in a different month.
    /// <para>
    /// Given a date, this method returns a business day. <br/>
    /// If the input date is a business day, it is returned. <br/>
    /// If the next business day is within the same month, it is returned. <br/>
    /// Otherwise, the last business day of the month is returned.
    /// </para>
    /// <remarks>
    /// Note that the result of this method may be earlier than the input date. <br/>
    /// This corresponds to the 'BusinessDayModifiedFollowing' business day convention. <br/>
    /// </remarks>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <returns>The input date if it is a business day, the next business day if within the same month or the last business day of the month</returns>
    DateOnly NextSameOrLastInMonth(DateOnly date);

    /// <summary>
    /// Checks if the specified date is the last business day of the month.
    /// <para>
    /// This returns true if the date specified is the last valid business day of the month.
    /// </para>
    /// </summary>
    /// <param name="date">The date to check</param>
    /// <returns>True if the specified date is the last business day of the month</returns>
    bool IsLastBusinessDayOfMonth(DateOnly date);

    /// <summary>
    /// Calculates the last business day of the month.
    /// <para>
    /// Given a date, this method returns the date of the last business day of the month.
    /// </para>
    /// </summary>
    /// <param name="date">The date to check</param>
    /// <returns>The date of the last business day of the month</returns>
    DateOnly LastBusinessDayOfMonth(DateOnly date);

    /// <summary>
    /// Calculates the number of business days between two dates.
    /// <para>
    /// This calculates the number of business days within the range. <br/>
    /// If the dates are equal, zero is returned. <br/>
    /// If the end is before the start, an exception is thrown.
    /// </para>
    /// </summary>
    /// <param name="startInclusive">The start date</param>
    /// <param name="endExclusive">The end date</param>
    /// <returns>The total number of business days between the start and end date</returns>
    int DaysBetween(DateOnly startInclusive, DateOnly endExclusive);

    /// <summary>
    /// Gets the stream of business days between the two dates.
    /// </summary>
    /// <param name="startInclusive">The start date</param>
    /// <param name="endExclusive">The end date</param>
    IEnumerable<DateOnly> BusinessDays(DateOnly startInclusive, DateOnly endExclusive);

    /// <summary>
    /// Gets the stream of holidays between the two dates.
    /// </summary>
    /// <param name="startInclusive">The start date</param>
    /// <param name="endExclusive">The end date</param>
    /// <returns></returns>
    IEnumerable<DateOnly> Holidays(DateOnly startInclusive, DateOnly endExclusive);
}