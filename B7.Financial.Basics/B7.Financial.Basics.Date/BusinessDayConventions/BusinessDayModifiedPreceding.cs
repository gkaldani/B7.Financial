using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date.BusinessDayConventions;
using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// The 'ModifiedPreceding' convention which adjusts to the previous business day without crossing month start.
/// <para>
/// If the input date is not a business day then the date is adjusted. <br/>
/// The adjusted date is the previous business day unless that day is in a different <br/>
/// calendar month, in which case the next business day is returned.
/// </para>
/// </summary>
public sealed class BusinessDayModifiedPreceding : BusinessDayConvention
{
    /// <summary>
    /// The name of the Business Day Convention.
    /// </summary>
    public static readonly Name BusinessDayConventionName = "ModifiedPreceding";


    /// <inheritdoc />
    public override Name Name => BusinessDayConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date, IHolidayCalendar calendar)
    {
        // previous business day unless over a month end
        var adjusted = calendar.PreviousOrSame(date);

        if (adjusted.Month != date.Month)
        {
            adjusted = calendar.Next(adjusted);
        }

        // Otherwise, return the adjusted date
        return adjusted;
    }
}