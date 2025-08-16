using System.Diagnostics.Contracts;

namespace B7.Financial.Basics.Date;

/// <summary>
/// DateOnlyUtils provides utility methods for working with <see cref="DateOnly"/> values.
/// </summary>
public static class DateOnlyUtils
{
    /// <summary>
    /// Returns the number of days between two <see cref="DateOnly"/> values.
    /// </summary>
    /// <param name="firstDate">The first date.</param>
    /// <param name="secondDate">The second date.</param>
    /// <returns>The number of days between the two dates.</returns>
    public static int DaysBetween(DateOnly firstDate, DateOnly secondDate) =>
        secondDate.DayNumber - firstDate.DayNumber;

    /// <summary>
    /// Returns the number of days in a given year.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static int DaysInYear(int year) => DateTime.IsLeapYear(year) ? 366 : 365;

    /// <summary>
    /// Returns the number of days in the year of a given <see cref="DateOnly"/> value.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int DaysInYear(this DateOnly date) => DateTime.IsLeapYear(date.Year) ? 366 : 365;


    /// <summary>
    /// Generates a sequence of dates within the specified range.
    /// </summary>
    /// <remarks>The method iterates through the range of dates, incrementing by one day at a time, and yields
    /// each date in the sequence.</remarks>
    /// <param name="startInclusive">The start date of the range, inclusive.</param>
    /// <param name="endExclusive">The end date of the range, exclusive.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="DateOnly"/> representing each date from <paramref
    /// name="startInclusive"/> to the day before <paramref name="endExclusive"/>.</returns>
    public static IEnumerable<DateOnly> Stream(DateOnly startInclusive, DateOnly endExclusive)
    {
        for (var current = startInclusive; current < endExclusive; current = current.AddDays(1))
        {
            yield return current;
        }
    }

    /// <summary>
    /// Checks if the specified <see cref="DateOnly"/> is before another date.
    /// </summary>
    public static bool IsAfter(this DateOnly self, DateOnly date) => self > date;

    /// <summary>
    /// Checks if the specified <see cref="DateOnly"/> is before another date.
    /// </summary>
    public static bool IsBefore(this DateOnly self, DateOnly date) => self < date;

    /// <summary>
    /// Checks if the specified <see cref="DateOnly"/> is before another date.
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsLeapYear(this DateOnly self) => DateTime.IsLeapYear(self.Year);

    /// <summary>
    /// Returns the number of days in the month represented by the current <see cref="DateOnly"/> instance.
    /// </summary>
    /// <remarks>This method uses the year and month of the <see cref="DateOnly"/> instance to determine the
    /// number of days. For example, it accounts for leap years when calculating the days in February.</remarks>
    /// <param name="self"></param>
    /// <returns>The total number of days in the month for the specified year and month.</returns>
    public static int DaysInMonth(this DateOnly self) => DateTime.DaysInMonth(self.Year, self.Month);

    /// <summary>
    /// Returns a <see cref="DateOnly"/> instance representing the last day of the month for the specified date.
    /// </summary>
    /// <remarks>This method calculates the last day of the month based on the year and month of the provided
    /// <paramref name="self"/>.</remarks>
    /// <param name="self">The <see cref="DateOnly"/> instance for which to determine the last day of the month.</param>
    /// <returns>A <see cref="DateOnly"/> representing the last day of the month for the specified date.</returns>
    public static DateOnly LastDayOfMonth(this DateOnly self) => new(self.Year, self.Month, self.DaysInMonth());

    /// <summary>
    /// Returns the start of the month using the specified calendar.
    /// </summary>
    /// <param name="date">The <see cref="DateOnly"/> to get the start of the month for.</param>
    /// <returns>A <see cref="DateOnly"/> structure.</returns>
    [Pure]
    public static DateOnly StartOfMonth(this DateOnly date)
    {
        var year = date.Year;
        var month = date.Month;
        return new DateOnly(year, month, 1);
    }

    /// <summary>
    /// Returns the requested occurrence of the specified week day in the month using the specified calendar.
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> to get the day from.</param>
    /// <param name="occurrence">The occurrence of the week day to return.</param>
    /// <param name="dayOfWeek">The <see cref="DayOfWeek"/> to return.</param>
    /// <returns>A <see cref="DateTime"/> structure.</returns>
    [Pure]
    public static DateOnly DayOfWeekInMonth(this DateOnly date, int occurrence, DayOfWeek dayOfWeek)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(occurrence);

        var day = date.StartOfMonth();

        for (var i = 0; i < occurrence; i++, day = day.AddDays(1))
        {
            while (day.DayOfWeek != dayOfWeek)
                day = day.AddDays(1);

            // it's not known how many weeks may be in the month, but once we've past the
            // original month we're out of range
            if (day.Month != date.Month)
                throw new ArgumentOutOfRangeException(nameof(occurrence));
        }

        // account for additional day added by loop increment
        return day.AddDays(-1);
    }

    /// <summary>
    /// Returns the next date that matches the specified day of the week, or the same date if it already matches.
    /// </summary>
    [Pure]
    public static DateOnly NextOrSame(this DateOnly date, DayOfWeek dayOfWeek)
    {
        if (date.DayOfWeek == dayOfWeek)
            return date;

        for (var i = 1; i < 7; i++)
        {
            var calculated = date.AddDays(i);

            if (calculated.DayOfWeek == dayOfWeek)
                return calculated;
        }

        return date;
    }

    /// <summary>
    /// Returns the previous date that matches the specified day of the week, or the same date if it already matches.
    /// </summary>
    [Pure]
    public static DateOnly PreviousOrSame(this DateOnly date, DayOfWeek dayOfWeek)
    {
        if (date.DayOfWeek == dayOfWeek)
            return date;

        for (var i = 1; i < 7; i++)
        {
            var calculated = date.AddDays(-i);

            if (calculated.DayOfWeek == dayOfWeek)
                return calculated;
        }

        return date;
    }
}