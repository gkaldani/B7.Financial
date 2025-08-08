using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// Actual/Actual ISDA day count convention.
/// </summary>
public sealed class DayCountActualActualIsda : DayCount, INamed
{
    public new static string Name => "Act/Act ISDA";

    public override string GetName() => Name;

    public override decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IScheduleInfo? scheduleInfo)
    {
        if (firstDate > secondDate)
            throw new ArgumentException("The first date must be earlier than the second date.");

        var y1 = firstDate.Year;
        var y2 = secondDate.Year;

        decimal firstYearLength = DateOnlyUtils.DaysInYear(y1);
        if (y1 == y2)
        {
            decimal actualDays = secondDate.DayOfYear - firstDate.DayOfYear;
            // If both dates are in the same year, return the fraction of days in that year
            return actualDays / firstYearLength;
        }

        decimal firstRemainderOfYear = firstYearLength - firstDate.DayOfYear + 1;
        decimal secondRemainderOfYear = secondDate.DayOfYear - 1;
        var secondYearLength = DateOnlyUtils.DaysInYear(y2);

        return (firstRemainderOfYear / firstYearLength) +
               (secondRemainderOfYear / secondYearLength) +
               (y2 - y1 - 1);
    }

    public override int Days(DateOnly firstDate, DateOnly secondDate) =>
        DateOnlyUtils.DaysBetween(firstDate, secondDate);
}