namespace B7.Financial.Basics.Date;

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
}