using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// The 'Act/Act ICMA' day count, which divides the actual number of days by <br/>
/// the actual number of days in the coupon period multiplied by the frequency. <br/>
/// Also known as 'Actual/Actual ICMA' or 'Actual/Actual (Bond)'. <br/>
/// Defined by the 2006 ISDA definitions 4.16c and ICMA rule 251.1(iii) and 251.3
/// <para>
/// The result is calculated as follows.
/// </para>
/// <para>
/// First, the underlying schedule period is obtained treating the first date as the start of the schedule period.
/// </para>
/// <para>
/// Second, if the period is a stub, then nominal regular periods are created matching the <br/>
/// schedule frequency, working forwards or backwards from the known regular schedule date. <br/>
/// An end-of-month flag is used to handle month-ends. <br/>
/// If the period is not a stub then the schedule period treated as a nominal period below.
/// </para>
/// <para>
/// Third, the result is calculated as the sum of a calculation for each nominal period. <br/>
/// The actual days between the first and second date are allocated to the matching nominal period. <br/>
/// Each calculation is a division. The numerator is the actual number of days in <br/>
/// the nominal period, which could be zero in the case of a long stub. <br/>
/// The denominator is the length of the nominal period  multiplied by the frequency. <br/>
/// The first day in the period is included, the last day is excluded. <br/>
/// </para>
/// <para>
/// Due to the way that the nominal periods are determined ignoring business day adjustments, <br/>
/// this day count is recommended for use by bonds, not swaps.
/// </para>
/// <para>
/// The method <see cref="DayCount.YearFraction(System.DateOnly,System.DateOnly)"/> will throw an <br/>
/// exception because schedule information is required for this day count. <br/>
/// as later clarified by ISDA 'EMU and market conventions' http://www.isda.org/c_and_a/pdf/mktc1198.pdf.
/// </para>
/// </summary>
public sealed class DayCountActualActualIcma : DayCount
{
    private static readonly Name DayCountName = "Act/Act ICMA";
    public override Name Name => DayCountName;

    public override decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IDayCount.IScheduleInfo? scheduleInfo)
    {
        // avoid using ScheduleInfo in this case
        if (firstDate == secondDate)
        {
            return 0;
        }

        if (scheduleInfo is null)
            throw new ArgumentNullException(nameof(scheduleInfo), "Schedule information is required for this day count.");


        // calculation is based on the schedule period, firstDate assumed to be the start of the period
        var scheduleEndDate = scheduleInfo.EndDate;
        var nextCouponDate = scheduleInfo.PeriodEndDate(firstDate);

        return 0;
    }

    public override int Days(DateOnly firstDate, DateOnly secondDate) =>
        DateOnlyUtils.DaysBetween(firstDate, secondDate);
}