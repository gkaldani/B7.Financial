using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// The 'Act/365 Actual' day count convention. <br/>
/// Also known as 'Act/365A'. <be/>
/// Which divides the actual number of days by 366 if the period contains 'Leap Day' (February 29th), <br/>
/// or by 365 if it does not.
/// <para>
/// The result is a simple division. <br/>
/// The numerator is the actual number of days in the requested period. <br/>
/// The denominator is 366 if the period contains leap day (February 29th), if not it is 365. <br/>
/// The first day in the period is excluded, the last day is included.
/// </para>
/// </summary>
public sealed class DayCountActual365Actual : DayCount
{
    /// <summary>
    /// The name of the day count convention.
    /// </summary>
    public static readonly Name DayCountName = "Act/365A";

    /// <inheritdoc />
    public override Name Name => DayCountName;

    /// <inheritdoc />
    public override decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IDayCount.IScheduleInfo? scheduleInfo)
    {
        if (firstDate > secondDate)
            throw new ArgumentException("The first date must be earlier than the second date.");

        var actualDays = Days(firstDate, secondDate);
        //Check period contains leap day
        var denominator = firstDate.NextLeapDay().IsAfter(secondDate) ? 366m : 365m;

        return actualDays / denominator;
    }

    /// <inheritdoc />
    public override int Days(DateOnly firstDate, DateOnly secondDate) =>
        DateOnlyUtils.DaysBetween(firstDate, secondDate);
}