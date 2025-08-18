using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// The 'ModifiedFollowingBiMonthly' convention which adjusts to the next business day without <br/>
/// crossing mid-month or month end.
/// <para>
/// If the input date is not a business day then the date is adjusted. <br/>
/// The month is divided into two parts, the first half, the 1st to 15th and the 16th onwards. <br/>
/// The adjusted date is the next business day unless that day is in a different half-month, <br/>
/// in which case the previous business day is returned.
/// </para>
/// </summary>
public sealed class BusinessDayModifiedFollowingBiMonthly : BusinessDayConvention
{
    /// <summary>
    /// The name of the Business Day Convention.
    /// </summary>
    public static readonly Name BusinessDayConventionName = "ModifiedFollowingBiMonthly";


    /// <inheritdoc />
    public override Name Name => BusinessDayConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date, IHolidayCalendar calendar)
    {
        var adjusted = calendar.NextOrSame(date);
        
        if (adjusted.Month != date.Month || (adjusted.Day > 15 && date.Day <= 15))
        {
            // If the adjusted date is in a different month or crosses the 15th day of the month,
            adjusted = calendar.Previous(adjusted);
        }

        // Otherwise, return the adjusted date
        return adjusted;
    }
}