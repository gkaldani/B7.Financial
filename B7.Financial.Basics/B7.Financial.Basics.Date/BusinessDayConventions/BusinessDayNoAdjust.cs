using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// The 'NoAdjust' convention which makes no adjustment.
/// <para>
/// The input date will not be adjusted even if it is not a business day.
/// </para>
/// </summary>
public sealed class BusinessDayNoAdjust : BusinessDayConvention
{
    /// <summary>
    /// No adjustment is made to the date, even if it is not a business day.
    /// </summary>
    public static readonly IBusinessDayConvention NoAdjust = new BusinessDayNoAdjust();
    
    /// <summary>
    /// The name of the Business Day Convention.
    /// </summary>
    public static readonly Name BusinessDayConventionName = "NoAdjust";


    /// <inheritdoc />
    public override Name Name => BusinessDayConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date, IHolidayCalendar calendar) => date;
}