using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Basics.Date.PeriodAdditionConventions;

/// <summary>
/// No specific rule applies.
/// <para>
/// Given a date, the specified period is added using standard date arithmetic. <br/>
/// The business day adjustment is applied to produce the final result.
/// </para>
/// <remarks>
/// For example, adding a period of 1 month to June 30th will result in July 30th.
/// </remarks>
/// </summary>
public sealed class PeriodAdditionNone : IPeriodAdditionConvention
{
    /// <summary>
    /// The name of the Period Addition Convention.
    /// </summary>
    public static readonly Name PeriodAdditionName = "None";


    /// <inheritdoc />
    public Name Name => PeriodAdditionName;

    /// <inheritdoc />
    public DateOnly Adjust(DateOnly date, Period period, IHolidayCalendar holidayCalendar) =>
        period.AddTo(date);

    /// <inheritdoc />
    public bool IsMonthBased => false;
}