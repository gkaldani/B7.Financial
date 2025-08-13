using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Schedule;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// The 'Act/365 Leap Yea Year' day count convention. <br/>
/// Also known as 'Act/365L'. <be/>
/// Which divides the actual number of days by 365 or 366. <br/>
/// <para>
/// The result is a simple division. <br/>
/// The numerator is the actual number of days in the requested period. <br/>
/// The denominator is determined by examining the frequency and the period end date (the date of the next coupon). <br/>
/// If the frequency is annual then the denominator is 366 if the period contains 'Leap Day' (February 29th), <br/>
/// if not it is 365. The first day in the period is excluded, the last day is included. <br/>
/// If the frequency is not annual, the denominator is 366 if the period end date <br/>
/// is in a leap year, if not it is 365.
/// </para>
/// <remarks>
/// Defined by the 2006 ISDA definitions 4.16i and ICMA rule 251.1(i) part 2 as later clarified by ICMA and Swiss Exchange.
/// </remarks>
/// </summary>
public sealed class DayCountActual365LeapYear : DayCount
{
    /// <summary>
    /// The name of the day count convention.
    /// </summary>
    public static readonly Name DayCountName = "Act/365L";

    /// <inheritdoc />
    public override Name Name => DayCountName;

    /// <inheritdoc />
    public override decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IDayCount.IScheduleInfo? scheduleInfo)
    {
        if (firstDate > secondDate)
            throw new ArgumentException("The first date must be earlier than the second date.");

        if (scheduleInfo is null)
            throw new ArgumentNullException(nameof(scheduleInfo), "Schedule info cannot be null.");

        var actualDays = Days(firstDate, secondDate);

        if (actualDays == 0)
            return 0m;

        // calculation is based on the end of the schedule period (next coupon date) and annual/non-annual frequency
        var nextCouponDate = scheduleInfo.PeriodEndDate(firstDate);

        if (nextCouponDate is null)
            throw new ArgumentException("The schedule info must provide a valid period end date.", nameof(scheduleInfo));

        if (scheduleInfo.Frequency.IsAnnual)
        {
            return actualDays / (firstDate.NextLeapDay().IsAfter(nextCouponDate.Value) ? 366m : 365m);
        }
        else
        {
            return actualDays / (nextCouponDate.Value.IsLeapYear() ? 366m : 365m);
        }
    }

    /// <inheritdoc />
    public override int Days(DateOnly firstDate, DateOnly secondDate) =>
        DateOnlyUtils.DaysBetween(firstDate, secondDate);
}