using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Schedule;
using System.Collections.Frozen;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <summary>
/// Represents a factory for creating standard roll conventions.
/// </summary>
public class StandardRollConventionsFactory : IRollConventionFactory
{
    /// <summary>No Adjustment</summary>
    public static readonly RollConventionNone None = new();

    /// <summary>Day of Month (DOM) roll convention for first (1) day of the month.</summary>
    public static readonly RollConventionDom1 Dom1 = new ();

    /// <summary>Day of Month (DOM) roll convention for second (2) day of the month.</summary>
    public static readonly RollConventionDom2 Dom2 = new ();

    /// <summary>Day of Month (DOM) roll convention for third (3) day of the month.</summary>
    public static readonly RollConventionDom3 Dom3 = new ();

    /// <summary>Day of Month (DOM) roll convention for fourth (4) day of the month.</summary>
    public static readonly RollConventionDom4 Dom4 = new ();

    /// <summary>Day of Month (DOM) roll convention for fifth (5) day of the month.</summary>
    public static readonly RollConventionDom5 Dom5 = new ();

    /// <summary>Day of Month (DOM) roll convention for sixth (6) day of the month.</summary>
    public static readonly RollConventionDom6 Dom6 = new ();

    /// <summary>Day of Month (DOM) roll convention for seventh (7) day of the month.</summary>
    public static readonly RollConventionDom7 Dom7 = new ();

    /// <summary>Day of Month (DOM) roll convention for eighth (8) day of the month.</summary>
    public static readonly RollConventionDom8 Dom8 = new ();

    /// <summary>Day of Month (DOM) roll convention for ninth (9) day of the month.</summary>
    public static readonly RollConventionDom9 Dom9 = new ();

    /// <summary>Day of Month (DOM) roll convention for tenth (10) day of the month.</summary>
    public static readonly RollConventionDom10 Dom10 = new ();

    /// <summary>Day of Month (DOM) roll convention for eleventh (11) day of the month.</summary>
    public static readonly RollConventionDom11 Dom11 = new ();

    /// <summary>Day of Month (DOM) roll convention for twelfth (12) day of the month.</summary>
    public static readonly RollConventionDom12 Dom12 = new ();

    /// <summary>Day of Month (DOM) roll convention for thirteenth (13) day of the month.</summary>
    public static readonly RollConventionDom13 Dom13 = new ();

    /// <summary>Day of Month (DOM) roll convention for fourteenth (14) day of the month.</summary>
    public static readonly RollConventionDom14 Dom14 = new ();

    /// <summary>Day of Month (DOM) roll convention for fifteenth (15) day of the month.</summary>
    public static readonly RollConventionDom15 Dom15 = new ();

    /// <summary>Day of Month (DOM) roll convention for sixteenth (16) day of the month.</summary>
    public static readonly RollConventionDom16 Dom16 = new ();

    /// <summary>Day of Month (DOM) roll convention for seventeenth (17) day of the month.</summary>
    public static readonly RollConventionDom17 Dom17 = new ();

    /// <summary>Day of Month (DOM) roll convention for eighteenth (18) day of the month.</summary>
    public static readonly RollConventionDom18 Dom18 = new ();

    /// <summary>Day of Month (DOM) roll convention for nineteenth (19) day of the month.</summary>
    public static readonly RollConventionDom19 Dom19 = new ();

    /// <summary>Day of Month (DOM) roll convention for twentieth (20) day of the month.</summary>
    public static readonly RollConventionDom20 Dom20 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-first (21) day of the month.</summary>
    public static readonly RollConventionDom21 Dom21 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-second (22) day of the month.</summary>
    public static readonly RollConventionDom22 Dom22 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-third (23) day of the month.</summary>
    public static readonly RollConventionDom23 Dom23 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-fourth (24) day of the month.</summary>
    public static readonly RollConventionDom24 Dom24 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-fifth (25) day of the month.</summary>
    public static readonly RollConventionDom25 Dom25 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-sixth (26) day of the month.</summary>
    public static readonly RollConventionDom26 Dom26 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-seventh (27) day of the month.</summary>
    public static readonly RollConventionDom27 Dom27 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-eighth (28) day of the month.</summary>
    public static readonly RollConventionDom28 Dom28 = new ();

    /// <summary>Day of Month (DOM) roll convention for twenty-ninth (29) day of the month.</summary>
    public static readonly RollConventionDom29 Dom29 = new ();

    /// <summary>Day of Month (DOM) roll convention for thirtieth (30) day of the month.</summary>
    public static readonly RollConventionDom30 Dom30 = new ();

    /// <summary>Last Day of Month (EOM) roll convention.</summary>
    public static readonly RollConventionEom Eom = new();


    /// <summary>Day of Week (DOW) roll convention for Monday.</summary>
    public static readonly RollConventionDowMonday Monday = new ();

    /// <summary>Day of Week (DOW) roll convention for Tuesday.</summary>
    public static readonly RollConventionDowTuesday Tuesday = new ();

    /// <summary>Day of Week (DOW) roll convention for Wednesday.</summary>
    public static readonly RollConventionDowWednesday Wednesday = new ();

    /// <summary>Day of Week (DOW) roll convention for Thursday.</summary>
    public static readonly RollConventionDowThursday Thursday = new ();

    /// <summary>Day of Week (DOW) roll convention for Friday.</summary>
    public static readonly RollConventionDowFriday Friday = new ();

    /// <summary>Day of Week (DOW) roll convention for Saturday.</summary>
    public static readonly RollConventionDowSaturday Saturday = new ();

    /// <summary>Day of Week (DOW) roll convention for Sunday.</summary>
    public static readonly RollConventionDowSunday Sunday = new ();

    /// <summary> 3rd Wednesday </summary>
    public static readonly RollConventionImm Imm = new();

    /// <summary> Wednesday on or after 9th </summary>
    public static readonly RollConventionImmnzd Immnzd = new();

    /// <summary> 2nd Friday </summary>
    public static readonly RollConventionSfe Sfe = new();

    private static readonly FrozenDictionary<Name, RollConvention> RollConventions =
        new Dictionary<Name, RollConvention>
        {
            [None.Name] = None,

            [Dom1.Name] = Dom1, [Dom2.Name] = Dom2, [Dom3.Name] = Dom3, [Dom4.Name] = Dom4, [Dom5.Name] = Dom5,
            [Dom6.Name] = Dom6, [Dom7.Name] = Dom7, [Dom8.Name] = Dom8, [Dom9.Name] = Dom9, [Dom10.Name] = Dom10,
            [Dom11.Name] = Dom11, [Dom12.Name] = Dom12, [Dom13.Name] = Dom13, [Dom14.Name] = Dom14, [Dom15.Name] = Dom15,
            [Dom16.Name] = Dom16, [Dom17.Name] = Dom17, [Dom18.Name] = Dom18, [Dom19.Name] = Dom19, [Dom20.Name] = Dom20,
            [Dom21.Name] = Dom21, [Dom22.Name] = Dom22, [Dom23.Name] = Dom23, [Dom24.Name] = Dom24, [Dom25.Name] = Dom25,
            [Dom26.Name] = Dom26, [Dom27.Name] = Dom27, [Dom28.Name] = Dom28, [Dom29.Name] = Dom29, [Dom30.Name] = Dom30,
            [Eom.Name] = Eom,

            [Monday.Name] = Monday, [Tuesday.Name] = Tuesday, [Wednesday.Name] = Wednesday,
            [Thursday.Name] = Thursday, [Friday.Name] = Friday, [Saturday.Name] = Saturday, [Sunday.Name] = Sunday,

            [Imm.Name] = Imm,
            [Immnzd.Name] = Immnzd,
            [Sfe.Name] = Sfe

        }.ToFrozenDictionary();

    /// <summary>
    /// Retrieves the <see cref="IRollConvention"/> by its name.
    /// </summary>
    /// <param name="name">The name of the Roll Convention to retrieve.</param>
    /// <returns>The roll convention associated with the specified <paramref name="name"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified <paramref name="name"/> does not correspond to a known roll convention.</exception>
    public virtual IRollConvention Of(Name name) =>
        RollConventions.TryGetValue(name, out var convention)
            ? convention
            : throw new ArgumentException($"Unknown roll convention: {name}", nameof(name));

    /// <inheritdoc />
    public static IRollConvention OfDayOfMonth(byte dayOfMonth) => dayOfMonth switch
    {
        1 => Dom1,
        2 => Dom2,
        3 => Dom3,
        4 => Dom4,
        5 => Dom5,
        6 => Dom6,
        7 => Dom7,
        8 => Dom8,
        9 => Dom9,
        10 => Dom10,
        11 => Dom11,
        12 => Dom12,
        13 => Dom13,
        14 => Dom14,
        15 => Dom15,
        16 => Dom16,
        17 => Dom17,
        18 => Dom18,
        19 => Dom19,
        20 => Dom20,
        21 => Dom21,
        22 => Dom22,
        23 => Dom23,
        24 => Dom24,
        25 => Dom25,
        26 => Dom26,
        27 => Dom27,
        28 => Dom28,
        29 => Dom29,
        30 => Dom30,
        31 => Eom,
        _ => throw new ArgumentOutOfRangeException(nameof(dayOfMonth), dayOfMonth, "Day of month must be between 1 and 30 or EOM (31).")
    };

    /// <inheritdoc />
    public static IRollConvention OfDayOfWeek(DayOfWeek dayOfWeek) =>
        dayOfWeek switch
        {
            DayOfWeek.Monday => new RollConventionDowMonday(),
            DayOfWeek.Tuesday => new RollConventionDowTuesday(),
            DayOfWeek.Wednesday => new RollConventionDowWednesday(),
            DayOfWeek.Thursday => new RollConventionDowThursday(),
            DayOfWeek.Friday => new RollConventionDowFriday(),
            DayOfWeek.Saturday => new RollConventionDowSaturday(),
            DayOfWeek.Sunday => new RollConventionDowSunday(),
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };

    /// <inheritdoc />
    public virtual IEnumerable<Name> RollConventionNames() => RollConventions.Keys;
}