using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <summary>
/// Represents the "End of Month" (EOM) roll convention, which adjusts dates to the last day of the month.
/// <para>
/// The input date will be adjusted ensure it is the last valid day of the month. <br/>
/// The year and month of the result date will be the same as the input date.
/// </para>
/// <para>
/// This convention is intended for use with periods that are a multiple of months.
/// </para>
/// </summary>
/// <remarks>This roll convention ensures that dates are adjusted to the last calendar day of the month. <br/>
/// It is commonly used in financial applications where end-of-month alignment is required.</remarks>
public sealed class RollConventionEom : RollConvention
{
    /// <summary>
    /// The name of the Roll Convention.
    /// </summary>
    public static readonly Name RollConventionName = "EOM";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date) => date.LastDayOfMonth();

    /// <inheritdoc />
    public override byte DayOfMonth => 31;
}