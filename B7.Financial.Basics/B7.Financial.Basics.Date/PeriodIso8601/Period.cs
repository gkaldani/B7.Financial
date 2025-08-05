using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace B7.Financial.Basics.Date.PeriodIso8601;

public readonly partial record struct Period :
    IAdditionOperators<Period, Period, Period>,
    IAdditiveIdentity<Period, Period>,
    IMinMaxValue<Period>
{
    [GeneratedRegex(@"^(?:P)?(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)W)?(?:(\d+)D)?$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant)]
    private static partial Regex PeriodRegex();
    public static Period Zero => new(0, 0, 0, 0);
    public static Period AdditiveIdentity => Zero;
    public static Period MaxValue { get; } = new(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
    public static Period MinValue { get; } = Zero;

    public int Years { get; }
    public int Months { get; }
    public int Weeks { get; }
    public int Days { get; }

    public Period(int years, int months, int weeks, int days)
    {
        Years = years;
        Months = months;
        Weeks = weeks;
        Days = days;
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder("P");

        if (Years != 0) sb.Append($"{Years}Y");
        if (Months != 0) sb.Append($"{Months}M");
        if (Weeks != 0) sb.Append($"{Weeks}W");
        if (Days != 0) sb.Append($"{Days}D");

        // If no parts, default to "P0D"
        if (sb.Length == 1) sb.Append("0D");

        return sb.ToString();
    }

    public static Period operator +(Period left, Period right) =>
        new(
            left.Years + right.Years,
            left.Months + right.Months,
            left.Weeks + right.Weeks,
            left.Days + right.Days);

    public static Period Parse(string periodIso8601)
    {
        if (string.IsNullOrWhiteSpace(periodIso8601))
            throw new ArgumentException("Period string cannot be empty.");

        var match = PeriodRegex().Match(periodIso8601);
        if (!match.Success)
            throw new FormatException("Invalid ISO 8601 period format.");

        return new Period(
            years:  ParsePart(match.Groups[1].Value),
            months: ParsePart(match.Groups[2].Value),
            weeks:  ParsePart(match.Groups[3].Value),
            days:   ParsePart(match.Groups[4].Value)
        );
    }

    public static bool TryParse(string periodIso8601, [NotNullWhen(true)] out Period? period)
    {
        period = null;
        if (string.IsNullOrWhiteSpace(periodIso8601))
            return false;

        var match = PeriodRegex().Match(periodIso8601);
        if (!match.Success)
            return false;

        period = new Period(
            years:  ParsePart(match.Groups[1].Value),
            months: ParsePart(match.Groups[2].Value),
            weeks:  ParsePart(match.Groups[3].Value),
            days:   ParsePart(match.Groups[4].Value)
        );

        return true;
    }

    private static int ParsePart(string value) => string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
}