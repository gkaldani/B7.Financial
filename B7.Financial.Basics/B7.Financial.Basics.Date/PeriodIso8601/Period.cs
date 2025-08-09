using B7.Financial.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace B7.Financial.Basics.Date.PeriodIso8601;

/// <summary>
/// Represents a period of time in ISO 8601 format.
/// </summary>
public readonly record struct Period : INamed, INamedFactory<Period>,
    IAdditionOperators<Period, Period, Period>,
    IAdditiveIdentity<Period, Period>,
    IMinMaxValue<Period>
{
    public string Name => ToString();

    public static Period Zero => default;
    public static Period AdditiveIdentity => Zero;
    public static Period MaxValue { get; } = new(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
    public static Period MinValue { get; } = Zero;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfDays(int days) => new(years: 0, months: 0, weeks: 0, days: days);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfMonths(int months) => new(years: 0, months: months, weeks: 0, days: 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfWeeks(int weeks) => new(years: 0, months: 0, weeks: weeks, days: 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfYears(int years) => new(years: years, months: 0, weeks: 0, days: 0);

    public int Years { get; }
    public int Months { get; }
    public int Weeks { get; }
    public int Days { get; }

    public Period(int years, int months, int weeks, int days)
    {
        // Single validation check with bitwise OR for better performance
        if ((years | months | weeks | days) < 0)
            throw new ArgumentException("All values must be non-negative.");

        this.Years = years;
        this.Months = months;
        this.Weeks = weeks;
        this.Days = days;
    }

    public Period Of(string name) => Parse(name);

    public override string ToString()
    {
        if (this == Zero) return "Zero";

        var sb = new StringBuilder("P");

        if (Years != 0) sb.Append(Years).Append('Y');
        if (Months != 0) sb.Append(Months).Append('M');
        if (Weeks != 0) sb.Append(Weeks).Append('W');
        if (Days != 0) sb.Append(Days).Append('D');

        // If no parts, default to "P0D"
        if (sb.Length == 1) sb.Append("0D");

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period operator +(Period left, Period right) =>
        new(
            left.Years + right.Years,
            left.Months + right.Months,
            left.Weeks + right.Weeks,
            left.Days + right.Days);

    public static Period Parse(string value)
    {
        if (TryParseCore(value.AsSpan(), out var period))
            return period.Value;

        throw new FormatException($"Invalid period format: '{value}'");
    }

    public static bool TryParse(string value, [NotNullWhen(true)] out Period? period) =>
        TryParseCore(value.AsSpan(), out period);

    private static bool TryParseCore(ReadOnlySpan<char> value, [NotNullWhen(true)] out Period? period)
    {
        period = null;

        if (value.IsEmpty || value.IsWhiteSpace())
            return false;

        if (value.Equals("ZERO", StringComparison.OrdinalIgnoreCase))
        {
            period = Zero;
            return true;
        }

        if (value.Length == 0 || char.ToUpperInvariant(value[0]) != 'P')
            return false;

        var span = value[1..]; // Skip the 'P'

        var years = 0;
        var months = 0;
        var weeks = 0;
        var days = 0;

        var numberStart = 0;
        var hasNumber = false;

        for (var i = 0; i <= span.Length; i++)
        {
            // Process at end of span or when we hit a letter
            if (i == span.Length || char.IsLetter(span[i]))
            {
                if (!hasNumber)
                {
                    if (i == span.Length) break; // End of string, no trailing number
                    return false; // Letter without preceding number
                }

                // Parse the number
                var numberSpan = span.Slice(numberStart, i - numberStart);
                if (!int.TryParse(numberSpan, out var number) || number < 0)
                    return false;

                // Process the unit letter (if we're not at the end)
                if (i < span.Length)
                {
                    var unit = char.ToUpperInvariant(span[i]);
                    switch (unit)
                    {
                        case 'Y':
                            if (years != 0) return false; // Duplicate
                            years = number;
                            break;
                        case 'M':
                            if (months != 0) return false; // Duplicate
                            months = number;
                            break;
                        case 'W':
                            if (weeks != 0) return false; // Duplicate
                            weeks = number;
                            break;
                        case 'D':
                            if (days != 0) return false; // Duplicate
                            days = number;
                            break;
                        default:
                            return false;
                    }
                }

                // Reset for next number
                numberStart = i + 1;
                hasNumber = false;
            }
            else if (char.IsDigit(span[i]))
            {
                if (hasNumber) continue;
                numberStart = i;
                hasNumber = true;
            }
            else
            {
                return false; // Invalid character
            }
        }

        period = new Period(years, months, weeks, days);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToTotalMonths() => (Years * 12) + Months;

    public Period ToNormalized()
    {
        if (Months < 12) return this;

        var (years, months) = Math.DivRem(Months, 12);
        return new Period(Years + years, months, Weeks, Days);
    }

    public void Deconstruct(out int years, out int months, out int weeks, out int days)
    {
        years = Years;
        months = Months;
        weeks = Weeks;
        days = Days;
    }
}