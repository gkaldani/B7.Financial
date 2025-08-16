using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Schedule;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <summary>
/// The 'None' roll convention.
/// <para>
/// The input date will not be adjusted.
/// </para>
/// <para>
/// When calculating a schedule, there will be no further adjustment after the <br/>
/// periodic frequency is added or subtracted.
/// </para>
/// </summary>
public sealed class RollConventionNone : RollConvention
{
    /// <summary>
    /// The name of the Roll Convention.
    /// </summary>
    public static readonly Name RollConventionName = "None";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date) => date;
}