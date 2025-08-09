using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// Actual/Actual ISDA day count convention. <br/>
/// Also known as 'Actual/Actual'. <br/>
/// Defined by the 2006 ISDA definitions 4.16b.
/// <para>
/// The 'Act/Act ISDA' day count, which divides the actual number of days in a <br/>
/// leap year by 366 and the actual number of days in a standard year by 365.
/// </para>
/// <para>
/// The result is calculated in two parts. <br/>
/// The actual number of days in the requested period that fall in a leap year is divided by 366. <br/>
/// The actual number of days in the requested period that fall in a standard year is divided by 365. <br/>
/// The result is the sum of the two. <br/>
/// The first day in the period is included, the last day excluded.
/// </para>
/// </summary>
public sealed class DayCountActualActualIsda : DayCount
{
    public new static string Name => "Act/Act ISDA";

    public override string GetName() => Name;

    public override decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IDayCount.IScheduleInfo? scheduleInfo)
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