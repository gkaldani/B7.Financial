using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Basics.Date.PeriodAdditionConventions;

/// <summary>
/// Convention applying a last <i>business</i> day of month rule.
/// <para>
/// Given a date, the specified period is added using standard date arithmetic, <br/>
/// shifting to the last business day of the month if the base date is the <br/>
/// last business day of the month. <br/>
/// The business day adjustment is applied to produce the final result. <br/>
/// </para>
/// <remarks>
/// For example, adding a period of 1 month to June 29th will result in July 31st <br/>
/// assuming that June 30th is not a valid business day and July 31st is.
/// </remarks>
/// </summary>
public sealed class PeriodAdditionLastBusinessDay : IPeriodAdditionConvention
{
    /// <summary>
    /// The name of the Period Addition Convention.
    /// </summary>
    public static readonly Name PeriodAdditionName = "LastBusinessDay";
    
    /// <inheritdoc />
    public Name Name => PeriodAdditionName;
    
    /// <inheritdoc />
    public DateOnly Adjust(DateOnly date, Period period, IHolidayCalendar calendar)
    {
        var endDate = period.AddTo(date);
        
        // If the base date is the last business day of the month, adjust to the last business day of the month of the end date
        if (calendar.IsLastBusinessDayOfMonth(date))
            return calendar.LastBusinessDayOfMonth(endDate);

        return endDate;
    }
    
    /// <inheritdoc />
    public bool IsMonthBased => true;
}