using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// The 'Following' convention which adjusts to the next business day.
/// <para>
/// If the input date is not a business day then the date is adjusted. <br/>
/// The adjusted date is the next business day.
/// </para>
/// </summary>
public sealed class BusinessDayFollowing : BusinessDayConvention
{
    /// <summary>
    /// The name of the Business Day Convention.
    /// </summary>
    public static readonly Name BusinessDayConventionName = "Following";


    /// <inheritdoc />
    public override Name Name => BusinessDayConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date, IHolidayCalendar calendar) => calendar.NextOrSame(date);
}