using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <summary>
/// Base class for roll conventions that adjust dates based on a specific day of the month (DOM).
/// </summary>
public abstract class RollConventionDom : RollConvention
{
    /// <summary>
    /// Gets the day of month for this roll convention.
    /// </summary>
    public abstract byte Dom { get; }

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date)
    {
        var year = date.Year;
        var month = date.Month;
        return new DateOnly(year, month, DayOfMonth);
    }

    /// <inheritdoc />
    public override bool Matches(DateOnly date) =>
        (date.Day == DayOfMonth) ||
        (date.Month == 2 && DayOfMonth >= date.DaysInMonth() && date.Day == date.DaysInMonth());

    /// <inheritdoc />
    public override byte DayOfMonth => Dom;
}

/// <summary> Day of Month (DOM) roll convention for first (1) day of the month. </summary>
public sealed class RollConventionDom1 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 1;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day1";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for second (2) day of the month. </summary>
public sealed class RollConventionDom2 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 2;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day2";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for third (3) day of the month. </summary>
public sealed class RollConventionDom3 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 3;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day3";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for fourth (4) day of the month. </summary>
public sealed class RollConventionDom4 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 4;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day4";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for fifth (5) day of the month. </summary>
public sealed class RollConventionDom5 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 5;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day5";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for sixth (6) day of the month. </summary>
public sealed class RollConventionDom6 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 6;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day6";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for seventh (7) day of the month. </summary>
public sealed class RollConventionDom7 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 7;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day7";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for eighth (8) day of the month. </summary>
public sealed class RollConventionDom8 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 8;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day8";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for ninth (9) day of the month. </summary>
public sealed class RollConventionDom9 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 9;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day9";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for tenth (10) day of the month. </summary>
public sealed class RollConventionDom10 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 10;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day10";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for eleventh (11) day of the month. </summary>
public sealed class RollConventionDom11 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 11;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day11";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twelfth (12) day of the month. </summary>
public sealed class RollConventionDom12 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 12;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day12";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for thirteenth (13) day of the month. </summary>
public sealed class RollConventionDom13 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 13;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day13";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for fourteenth (14) day of the month. </summary>
public sealed class RollConventionDom14 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 14;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day14";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for fifteenth (15) day of the month. </summary>
public sealed class RollConventionDom15 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 15;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day15";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for sixteenth (16) day of the month. </summary>
public sealed class RollConventionDom16 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 16;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day16";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for seventeenth (17) day of the month. </summary>
public sealed class RollConventionDom17 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 17;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day17";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for eighteenth (18) day of the month. </summary>
public sealed class RollConventionDom18 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 18;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day18";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for nineteenth (19) day of the month. </summary>
public sealed class RollConventionDom19 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 19;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day19";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twentieth (20) day of the month. </summary>
public sealed class RollConventionDom20 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 20;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day20";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-first (21) day of the month. </summary>
public sealed class RollConventionDom21 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 21;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day21";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-second (22) day of the month. </summary>
public sealed class RollConventionDom22 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 22;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day22";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-third (23) day of the month. </summary>
public sealed class RollConventionDom23 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 23;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day23";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-fourth (24) day of the month. </summary>
public sealed class RollConventionDom24 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 24;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day24";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-fifth (25) day of the month. </summary>
public sealed class RollConventionDom25 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 25;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day25";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-sixth (26) day of the month. </summary>
public sealed class RollConventionDom26 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 26;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day26";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-seventh (27) day of the month. </summary>
public sealed class RollConventionDom27 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 27;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day27";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-eighth (28) day of the month. </summary>
public sealed class RollConventionDom28 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 28;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day28";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for twenty-ninth (29) day of the month. </summary>
public sealed class RollConventionDom29 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 29;

    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day29";

    /// <inheritdoc />
    public override Name Name => RollConventionName;
}

/// <summary> Day of Month (DOM) roll convention for thirtieth (30) day of the month. </summary>
public sealed class RollConventionDom30 : RollConventionDom
{
    /// <inheritdoc />
    public override byte Dom => 30;
    /// <summary> The name of the Roll Convention. </summary>
    public static readonly Name RollConventionName = "Day30";
    /// <inheritdoc />
    public override Name Name => RollConventionName;
}