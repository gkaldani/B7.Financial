using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date.BusinessDayConventions;
using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// The 'Preceding' convention which adjusts to the previous business day.
/// <para>
/// If the input date is not a business day then the date is adjusted. <br/>
/// The adjusted date is the previous business day.
/// </para>
/// </summary>
public sealed class BusinessDayPreceding : BusinessDayConvention
{
    /// <summary>
    /// The name of the Business Day Convention.
    /// </summary>
    public static readonly Name BusinessDayConventionName = "Preceding";


    /// <inheritdoc />
    public override Name Name => BusinessDayConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date, IHolidayCalendar calendar) => calendar.PreviousOrSame(date);
}