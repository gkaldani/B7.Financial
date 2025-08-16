using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <summary>
/// The 'SFE' roll convention which adjusts the date to the second Friday.
/// <para>
/// The input date will be adjusted ensure it is the second Friday of the month. <br/>
/// The year and month of the result date will be the same as the input date.
/// </para>
/// <remarks>This convention is intended for use with periods that are a multiple of months.</remarks>
/// </summary>
public sealed class RollConventionSfe : RollConvention
{
    /// <summary>
    /// The name of the Roll Convention.
    /// </summary>
    public static readonly Name RollConventionName = "SFE";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date) => date.DayOfWeekInMonth(2, DayOfWeek.Friday);
}