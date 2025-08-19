namespace B7.Financial.Abstractions.Date;

/// <summary>
/// An adjustment that alters a date by adding a tenor.
/// <para>
/// This adjustment adds a <see cref="Tenor"/> to the input date using an addition convention, <br/>
/// followed by an adjustment to ensure the result is a valid business day.
/// </para>
/// <para>
/// Addition is performed using standard calendar addition. <br/>
/// It is not possible to add a number of business days using this class.
/// See <see cref="DaysAdjustment"/> for an alternative that can handle addition of business days.
/// </para>
/// <para>
/// There are two steps in the calculation: <br/>
/// In step one, the period is added using the specified <see cref="IPeriodAdditionConvention"/>. <br/>
/// In step two, the result of step one is optionally adjusted to be a business day using a <see cref="BusinessDayAdjustment"/>. <br/>
/// </para>
/// </summary>
/// <remarks>
/// For example, a rule represented by this class might be: <br/>
/// "the end date is 5 years after the start date, with end-of-month rule based on the last business day of the month, <rb/>
/// adjusted to be a valid London business day using the 'ModifiedFollowing' convention".
/// </remarks>
public sealed class TenorAdjustment
{

    /// <summary>
    /// The tenor to be added.
    /// <para>
    /// When the adjustment is performed, this tenor will be added to the input date.
    /// </para>
    /// </summary>
    public Tenor Tenor { get; }

    public IPeriodAdditionConvention AdditionConvention { get; }
}