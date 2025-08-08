using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.DayCountConventions;

public sealed class DayCountActualActualIcma : DayCount, INamed
{
    public new static string Name => "Act/Act ICMA";
    public override string GetName() => Name;

    public override decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IScheduleInfo? scheduleInfo)
    {
        // avoid using ScheduleInfo in this case
        if (firstDate == secondDate)
        {
            return 0;
        }

        if (scheduleInfo is null)
            throw new ArgumentNullException(nameof(scheduleInfo));


        // calculation is based on the schedule period, firstDate assumed to be the start of the period
        var scheduleEndDate = scheduleInfo.EndDate;
        var nextCouponDate = scheduleInfo.PeriodEndDate(firstDate);

        return 0;
    }

    public override int Days(DateOnly firstDate, DateOnly secondDate) =>
        DateOnlyUtils.DaysBetween(firstDate, secondDate);
}