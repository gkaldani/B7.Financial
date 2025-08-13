using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// Always returns a year fraction of 1 and a day count of 1.
/// </summary>
public sealed class DayCountOneOne : DayCount
{
    /// <summary>
    /// The name of the day count convention.
    /// </summary>
    public static readonly Name DayCountName = "1/1";

    /// <inheritdoc />
    public override Name Name => DayCountName;

    /// <inheritdoc />
    public override decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IDayCount.IScheduleInfo? scheduleInfo) => 1;

    /// <inheritdoc />
    public override int Days(DateOnly firstDate, DateOnly secondDate) => 1;
}