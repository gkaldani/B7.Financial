using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Schedule;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;


/// <summary>
/// Base class for roll conventions that adjust dates based on a specific day of the week (DOW).
/// </summary>
public abstract class RollConventionDow : RollConvention
{
    /// <summary>
    /// Gets the day of the week for this roll convention.
    /// </summary>
    public abstract DayOfWeek Dow { get; }

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date) => date.NextOrSame(Dow);

    /// <inheritdoc />
    public override bool Matches(DateOnly date) => date.DayOfWeek == Dow;
    /// <inheritdoc />
    public override DateOnly Next(DateOnly date, Frequency periodicFrequency) 
        => periodicFrequency.AddTo(date).NextOrSame(Dow);

    /// <inheritdoc />
    public override DateOnly Previous(DateOnly date, Frequency periodicFrequency) 
        => periodicFrequency.SubtractFrom(date).PreviousOrSame(Dow);
}

/// <summary> Day of Week (DOW) roll convention for Monday. </summary>
public sealed class RollConventionDowMonday : RollConventionDow
{
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Monday";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <summary> Monday is the day of the week for this roll convention. </summary>
    public override DayOfWeek Dow => DayOfWeek.Monday;
}

/// <summary> Day of Week (DOW) roll convention for Tuesday. </summary>
public sealed class RollConventionDowTuesday : RollConventionDow
{
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Tuesday";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <summary> Tuesday is the day of the week for this roll convention. </summary>
    public override DayOfWeek Dow => DayOfWeek.Tuesday;
}

/// <summary> Day of Week (DOW) roll convention for Wednesday. </summary>
public sealed class RollConventionDowWednesday : RollConventionDow
{
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Wednesday";

    /// <inheritdoc />
    public override Name Name => RollConventionName;

    /// <summary> Wednesday is the day of the week for this roll convention. </summary>
    public override DayOfWeek Dow => DayOfWeek.Wednesday;
}

/// <summary> Day of Week (DOW) roll convention for Thursday. </summary>
public sealed class RollConventionDowThursday : RollConventionDow
{
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Thursday";
    /// <inheritdoc />
    public override Name Name => RollConventionName;
    /// <summary> Thursday is the day of the week for this roll convention. </summary>
    public override DayOfWeek Dow => DayOfWeek.Thursday;
}

/// <summary> Day of Week (DOW) roll convention for Friday. </summary>
public sealed class RollConventionDowFriday : RollConventionDow
{
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Friday";
    /// <inheritdoc />
    public override Name Name => RollConventionName;
    /// <summary> Friday is the day of the week for this roll convention. </summary>
    public override DayOfWeek Dow => DayOfWeek.Friday;
}

/// <summary> Day of Week (DOW) roll convention for Saturday. </summary>
public sealed class RollConventionDowSaturday : RollConventionDow
{
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Saturday";
    /// <inheritdoc />
    public override Name Name => RollConventionName;
    /// <summary> Saturday is the day of the week for this roll convention. </summary>
    public override DayOfWeek Dow => DayOfWeek.Saturday;
}

/// <summary> Day of Week (DOW) roll convention for Sunday. </summary>
public sealed class RollConventionDowSunday : RollConventionDow
{
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Sunday";
    /// <inheritdoc />
    public override Name Name => RollConventionName;
    /// <summary> Sunday is the day of the week for this roll convention. </summary>
    public override DayOfWeek Dow => DayOfWeek.Sunday;
}
