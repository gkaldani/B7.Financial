using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Basics.Date.PeriodAdditionConventions;

/// <summary>
/// Convention applying a last day of month rule, <i>ignoring business days</i>.
/// <para>
/// Given a date, the specified period is added using standard date arithmetic, <br/>
/// shifting to the end-of-month if the base date is the last day of the month. <br/>
/// The business day adjustment is applied to produce the final result. <br/>
/// Note that this rule is based on the last day of the month, not the last business day of the month.
/// </para>
/// <remarks>
/// For example, adding a period of 1 month to June 30th will result in July 31st.
/// </remarks>
/// </summary>
public sealed class PeriodAdditionLastDay : IPeriodAdditionConvention
{
    /// <summary>
    /// The name of the Period Addition Convention.
    /// </summary>
    public static readonly Name PeriodAdditionName = "LastDay";
    
    /// <inheritdoc />
    public Name Name => PeriodAdditionName;
    
    /// <inheritdoc />
    public DateOnly Adjust(DateOnly date, Period period, IHolidayCalendar calendar)
    {
        var endDate = period.AddTo(date);

        // If the base date is the last day of the month, adjust to the last day of the month of the end date
        if (date.Day == date.DaysInMonth())
            return endDate.LastDayOfMonth();

        // Otherwise, just return the end date
        return endDate.LastDayOfMonth();
    }
    
    /// <inheritdoc />
    public bool IsMonthBased => true;
}