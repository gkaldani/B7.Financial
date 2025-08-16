using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <summary>
/// The 'IMM' roll convention which adjusts the date to the third Wednesday.
/// <para>
/// The input date will be adjusted ensure it is the third Wednesday of the month. <br/>
/// The year and month of the result date will be the same as the input date.
/// </para>
/// <remarks>
/// This convention is intended for use with periods that are a multiple of months.
/// </remarks>
/// </summary>
public sealed class RollConventionImm : RollConvention
{
    /// <summary>
    /// The name of the Roll Convention.
    /// </summary>
    public static readonly Name RollConventionName = "IMM";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date) => date.DayOfWeekInMonth(3, DayOfWeek.Wednesday);
}