using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <summary>
/// The 'IMMNZD' roll convention which adjusts the date to the first Wednesday <br/>
/// on or after the ninth day of the month.
/// <para>
/// The input date will be adjusted to the ninth day of the month, and then it will <br/>
/// be adjusted to be a Wednesday. If the ninth is a Wednesday, then that is returned. <br/>
/// The year and month of the result date will be the same as the input date.
/// </para>
/// <remarks>
/// This convention is intended for use with periods that are a multiple of months.
/// </remarks>
/// </summary>
public sealed class RollConventionImmnzd : RollConvention
{
    /// <summary>
    /// The name of the Roll Convention.
    /// </summary>
    public static readonly Name RollConventionName = "IMMNZD";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date)
    {
        // Adjust to the 9th of the month
        var dom9 = new DateOnly(date.Year, date.Month, day: 9);

        //Wednesday on or after 9th
        return dom9.NextOrSame(DayOfWeek.Wednesday);
    }
}