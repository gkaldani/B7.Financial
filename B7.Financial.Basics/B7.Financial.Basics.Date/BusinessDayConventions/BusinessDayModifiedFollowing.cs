using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// The 'ModifiedFollowing' convention which adjusts to the next business day without crossing month end.
/// <para>
/// If the input date is not a business day then the date is adjusted. <br/>
/// The adjusted date is the next business day unless that day is in a different <br/>
/// calendar month, in which case the previous business day is returned.
/// </para>
/// </summary>
public sealed class BusinessDayModifiedFollowing : BusinessDayConvention
{
    /// <summary>
    /// The name of the Business Day Convention.
    /// </summary>
    public static readonly Name BusinessDayConventionName = "ModifiedFollowing";


    /// <inheritdoc />
    public override Name Name => BusinessDayConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date, IHolidayCalendar calendar) => calendar.NextSameOrLastInMonth(date);
}