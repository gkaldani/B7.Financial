namespace B7.Financial.Basics.Date;

/// <summary>
/// Provides utility methods and predefined adjusters for working with leap days (February 29).
/// </summary>
/// <remarks>A leap day is February 29, which occurs only in leap years. Leap years are years divisible by 4,
/// except for years divisible by 100 but not divisible by 400. This class includes methods to calculate  the next leap
/// day on or after a given date, as well as predefined adjusters for use with date manipulation.</remarks>
public static class DateAdjusters
{
    /// <summary>
    /// Determines the next leap day that occurs after the specified date.
    /// </summary>
    /// <remarks>A leap day is February 29, which occurs only in leap years. This method accounts for leap
    /// year rules and ensures the returned date is valid.</remarks>
    /// <param name="date">The starting date to evaluate.</param>
    /// <returns>A <see cref="DateOnly"/> representing the next February 29 that occurs after the specified date. If the
    /// input date is already February 29, the same date is returned.</returns>
    public static DateOnly NextLeapDay(this DateOnly date)
    {
        // If the date occurs before February 29 in a leap year, return February 29 of that year
        if (DateTime.IsLeapYear(date.Year) && date < new DateOnly(date.Year, 2, 29))
        {
            return new DateOnly(date.Year, 2, 29);
        }

        // Otherwise calculate the next leap year and return its leap day
        var nextLeapYear = GetNextLeapYear(date.Year);
        return new DateOnly(nextLeapYear, 2, 29);
    }

    /// <summary>
    /// Adjusts the given date to the next leap day.
    /// </summary>
    /// <remarks>A leap day is February 29, which occurs only in leap years. If the provided date is already
    /// February 29, the adjuster will return the same date. Otherwise, it calculates the next occurrence of February 29
    /// after the given date.</remarks>
    public static readonly DateAdjuster NextLeapDayAdjuster = date => date.NextLeapDay();

    /// <summary>
    /// Determines the next leap day (February 29) that is on or after the specified date.
    /// </summary>
    /// <remarks>A leap day occurs only in leap years, which are years divisible by 4, except for years
    /// divisible by 100 but not divisible by 400. This method ensures that the returned date is always February 29 of a
    /// valid leap year.</remarks>
    /// <param name="date">The starting date to evaluate.</param>
    /// <returns>A <see cref="DateOnly"/> representing the next leap day (February 29) that is on or after the specified
    /// <paramref name="date"/>. If the specified date is already a leap day, the same date is returned.</returns>
    public static DateOnly NextOrSameLeapDay(this DateOnly date)
    {

        // If the date is already a leap day, return it
        if (date is { Day: 29, Month: 2 })
        {
            return date;
        }

        // Calculate the next leap year
        var year = date.Year;
        if (date.Month > 2 || !DateTime.IsLeapYear(year))
        {
            year = GetNextLeapYear(year);
        }

        // Return the next leap day
        return new DateOnly(year, 2, 29);
    }


    /// <summary>
    /// Adjusts the given date to the next leap day, or returns the same date if it is already a leap day.
    /// </summary>
    /// <remarks>A leap day is February 29, which occurs only in leap years. If the provided date is already
    /// February 29, the adjuster will return the same date. Otherwise, it calculates the next occurrence of February 29
    /// after the given date.</remarks>
    public static readonly DateAdjuster NextOrSameLeapDayAdjuster = date => date.NextOrSameLeapDay();

    // Get next leap year
    private static int GetNextLeapYear(int year)
    {
        // Compute the next year divisible by 4 using bitwise operations
        year += 4 - (year & 3);

        // Adjust for century years not divisible by 400
        //if (year % 100 == 0 && year % 400 != 0)
        if (!DateTime.IsLeapYear(year))
        {
            year += 4;
        }

        return year;
    }
}
