using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace B7.Financial.Abstractions.Date;

/// <summary>
/// Represents a period of time in ISO 8601 format (date components only).
/// Examples: P1Y, P6M, P3W, P10D, P1Y2M10D, P0D.
/// </summary>
public readonly struct Period : INamed, INamedFactory<Period>,
    IEquatable<Period>,
    ISpanParsable<Period>,
    ISpanFormattable,
    IAdditionOperators<Period, Period, Period>,
    IMultiplyOperators<Period, int, Period>,
    IAdditiveIdentity<Period, Period>,
    IMinMaxValue<Period>
{
    /// <summary>
    /// The unique name (identifier) of the instance of <see cref="Period"/>.
    /// </summary>
    public Name Name => (Name)ToString();

    /// <summary>
    /// Zero period, representing no time duration.
    /// </summary>
    public static Period Zero => default;

    /// <summary>
    /// Additive identity for <see cref="Period"/>.
    /// </summary>
    public static Period AdditiveIdentity => Zero;

    /// <summary>
    /// Maximum value for <see cref="Period"/>.
    /// </summary>
    public static Period MaxValue { get; } = new(years: int.MaxValue, months: int.MaxValue, weeks: 0, days: int.MaxValue);

    /// <summary>
    /// Minimum value for <see cref="Period"/>.
    /// </summary>
    public static Period MinValue { get; } = Zero;

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified days.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfDays(int days) => new(years: 0, months: 0,days: days);

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified months.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfMonths(int months) => new(years: 0, months: months, days: 0);

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified weeks.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfWeeks(int weeks) => new(weeks);

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified years.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfYears(int years) => new(years: years, months: 0, days: 0);

    /// <summary>Year component of the period.</summary>
    public int Years { get; }

    /// <summary>Month component of the period.</summary>
    public int Months { get; }

    /// <summary>Week component of the period.</summary>
    public int Weeks { get; }

    /// <summary>Day component of the period.</summary>
    public int Days { get; }

    /// <summary>
    /// Gets a value indicating whether this period has any non-zero components.
    /// </summary>
    public bool IsZero => (Years | Months | Weeks | Days) == 0;

    /// <summary>
    /// Gets a value indicating whether this period is normalized (Months &lt; 12).
    /// </summary>
    public bool IsNormalized => Months < 12;

    /// <summary>
    /// Indicates this is a week-based period (W only).
    /// </summary>
    public bool IsWeekBased => Weeks != 0 && (Years | Months | Days) == 0;

    /// <summary>
    /// Indicates this is a date-based period (Y/M/D only).
    /// </summary>
    public bool IsDateBased => Weeks == 0;

    /// <summary>
    /// Constructs a new instance of <see cref="Period"/> with the specified values.
    /// </summary>
    /// <exception cref="ArgumentException">All values must be non-negative.</exception>
    private Period(int years, int months, int weeks, int days)
    {
        // Single validation check with bitwise OR for better performance
        if ((years | months | weeks | days) < 0)
            throw new ArgumentException("All values must be non-negative.");

        if (weeks > 0 && (years != 0 || months != 0 || days != 0))
            throw new ArgumentException("Weeks cannot be combined with years, months, or days in ISO 8601 format.");

        Years = years;
        Months = months;
        Weeks = weeks;
        Days = days;
    }

    /// <summary>
    /// Constructs a new instance of <see cref="Period"/> with the specified number of weeks.
    /// </summary>
    public Period(int weeks) : this(years: 0, months: 0, weeks, days: 0) { }

    /// <summary>
    /// Constructs a new instance of <see cref="Period"/> with the specified number of years, months, and days.
    /// </summary>
    public Period(int years, int months, int days) : this(years, months, weeks: 0, days) { }

    /// <summary>
    /// Retrieves an instance of <see cref="Period"/> based on the provided name.
    /// </summary>
    public Period Of(Name name) => Parse(name);

    /// <inheritdoc />
    public bool Equals(Period other)
    {
        return Years == other.Years &&
               Months == other.Months &&
               Weeks == other.Weeks &&
               Days == other.Days;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Period other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Years, Months, Weeks, Days);

    /// <summary>
    /// Compares two periods for equality.
    /// </summary>
    public static bool operator ==(Period left, Period right) => left.Equals(right);

    /// <summary>
    /// Compares two periods for inequality.
    /// </summary>
    public static bool operator !=(Period left, Period right) => !left.Equals(right);

    /// <summary>
    /// Returns a string representation of the period in ISO 8601 format.
    /// Zero is "P0D".
    /// </summary>
    public override string ToString()
    {
        // Use TryFormat to avoid intermediate allocations when possible
        Span<char> buffer = stackalloc char[32]; // plenty for int.MaxValue components
        if (TryFormat(buffer, out var written, default, provider: null))
            return new string(buffer[..written]);

        // Fallback (should not hit with buffer size above)
        var sb = new StringBuilder("P");
        if (Years != 0) sb.Append(Years).Append('Y');
        if (Months != 0) sb.Append(Months).Append('M');
        if (Weeks != 0) sb.Append(Weeks).Append('W');
        if (Days != 0) sb.Append(Days).Append('D');
        if (sb.Length == 1) sb.Append("0D");
        return sb.ToString();
    }

    /// <summary>
    /// Adds two periods using checked arithmetic and normalizes months into years.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period operator +(Period left, Period right)
    {
        checked
        {
            var years = left.Years + right.Years;
            var months = left.Months + right.Months;
            var weeks = left.Weeks + right.Weeks;
            var days = left.Days + right.Days;

            if (months < 12) return new Period(years, months, weeks, days);

            // Normalize months into years
            var (y, m) = Math.DivRem(months, 12);
            years += y;
            months = m;

            return new Period(years, months, weeks, days);
        }
    }

    /// <summary>
    /// Multiplies a period by a non-negative integer factor using checked arithmetic and normalizes months.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period operator *(Period period, int factor)
    {
        if (factor < 0) throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be non-negative.");
        checked
        {
            var years = period.Years * factor;
            var months = period.Months * factor;
            var weeks = period.Weeks * factor;
            var days = period.Days * factor;

            if (months < 12) return new Period(years, months, weeks, days);

            // Normalize months into years
            var (y, m) = Math.DivRem(months, 12);
            years += y;
            months = m;

            return new Period(years, months, weeks, days);
        }
    }

    /// <summary>
    /// Multiplies a period by a non-negative integer factor (commutative).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period operator *(int factor, Period period) => period * factor;

    /// <summary>
    /// Parses a period from a string in ISO 8601 format.
    /// </summary>
    public static Period Parse(Name value)
    {
        if (TryParseCore(value.AsSpan(), provider: null, out var period))
            return period.Value;

        throw new FormatException($"Invalid period format: '{value}'");
    }

    /// <summary>
    /// Tries to parse a period from a <see cref="Name"/> in ISO 8601 format.
    /// </summary>
    public static bool TryParse(Name value, [NotNullWhen(true)] out Period? period) =>
        TryParseCore(value.AsSpan(), provider: null, out period);

    /// <summary>
    /// Tries to parse a period from a string in ISO 8601 format.
    /// </summary>
    public static bool TryParse(string value, [NotNullWhen(true)] out Period? period) =>
        TryParseCore(value.AsSpan(), provider: null, out period);

#pragma warning disable RCS1163
    // ReSharper disable once UnusedParameter.Local
    private static bool TryParseCore(ReadOnlySpan<char> value, IFormatProvider? provider, [NotNullWhen(true)] out Period? period)
#pragma warning restore RCS1163
    {
        period = null;

        // Trim surrounding whitespace
        while (!value.IsEmpty && char.IsWhiteSpace(value[0])) value = value[1..];
        while (!value.IsEmpty && char.IsWhiteSpace(value[^1])) value = value[..^1];

        if (value.IsEmpty)
            return false;

        // Backward-compatibility convenience
        if (value.Equals("ZERO", StringComparison.OrdinalIgnoreCase))
        {
            period = Zero;
            return true;
        }

        if (value.Length == 0 || char.ToUpperInvariant(value[0]) != 'P')
            return false;

        var span = value[1..]; // Skip the 'P'
        if (span.IsEmpty)
        {
            // "P" alone is not valid ISO; require at least one component (or "P0D")
            return false;
        }

        var years = 0;
        var months = 0;
        var weeks = 0;
        var days = 0;

        var numberStart = 0;
        var hasNumber = false;
        var anyComponent = false;
        var hasWeeksComponent = false;

        for (var i = 0; i <= span.Length; i++)
        {
            if (i == span.Length || char.IsLetter(span[i]))
            {
                if (!hasNumber)
                {
                    if (i == span.Length) break;
                    return false; // Letter without preceding number
                }

                // Parse the number
                var numberSpan = span.Slice(numberStart, i - numberStart);
                if (!int.TryParse(numberSpan, out var number) || number < 0)
                    return false;

                if (i == span.Length)
                {
                    // Trailing number with no unit (e.g. "P10")
                    return false;
                }

                var unit = char.ToUpperInvariant(span[i]);
                switch (unit)
                {
                    case 'Y':
                        if (years != 0) return false; // Duplicate
                        if (hasWeeksComponent) return false; // ISO: weeks cannot be mixed with Y/M/D
                        years = number;
                        break;
                    case 'M':
                        if (months != 0) return false; // Duplicate
                        if (hasWeeksComponent) return false;
                        months = number;
                        break;
                    case 'W':
                        if ((years | months | days) != 0) return false; // ISO: W cannot mix with Y/M/D
                        if (weeks != 0) return false; // Duplicate
                        weeks = number;
                        hasWeeksComponent = true;
                        break;
                    case 'D':
                        if (days != 0) return false; // Duplicate
                        if (hasWeeksComponent) return false;
                        days = number;
                        break;
                    default:
                        return false;
                }

                anyComponent = true;

                // Reset for next number
                numberStart = i + 1;
                hasNumber = false;
            }
            else if (char.IsDigit(span[i]))
            {
                if (hasNumber) continue;

                // Start of a new number
                numberStart = i;
                hasNumber = true;
            }
            else
            {
                return false; // Invalid character
            }
        }

        if (!anyComponent)
            return false;

        period = new Period(years, months, weeks, days);
        return true;
    }

    /// <summary>
    /// Returns the total number of months represented by this period (ignores weeks and days).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToTotalMonths() => (Years * 12) + Months;

    /// <summary>
    /// Normalizes the period by converting months greater than or equal to 12 into years.
    /// Weeks and days are unchanged.
    /// </summary>
    public Period ToNormalized()
    {
        if (Months < 12) return this;

        var (years, months) = Math.DivRem(Months, 12);
        return new Period(Years + years, months, Weeks, Days);
    }

    /// <summary>
    /// Deconstructs the period into its components.
    /// </summary>
    public void Deconstruct(out int years, out int months, out int weeks, out int days)
    {
        years = Years;
        months = Months;
        weeks = Weeks;
        days = Days;
    }

    /// <summary>
    /// Parses a period from a string in ISO 8601 format.
    /// </summary>
    public static Period Parse(string s, IFormatProvider? provider = null)
    {
        if (TryParseCore(s.AsSpan(), provider, out var period))
            return period.Value;

        throw new FormatException($"Invalid period format: '{s}'");
    }

    /// <summary>
    /// Parses a period from a span of characters in ISO 8601 format.
    /// </summary>
    public static Period Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (TryParseCore(s, provider, out var period))
            return period.Value;

        throw new FormatException($"Invalid period format: '{s.ToString()}'");
    }

    /// <summary>
    /// Tries to parse a period from a string in ISO 8601 format.
    /// </summary>
    public static bool TryParse(string? s, IFormatProvider? provider, out Period result)
    {
        result = default;
        if (s is null) return false;

        if (!TryParseCore(s.AsSpan(), provider, out var period)) return false;

        result = period.Value;
        return true;
    }

    /// <summary>
    /// Tries to parse a period from a span of characters in ISO 8601 format.
    /// </summary>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Period result)
    {
        result = default;

        if (!TryParseCore(s, provider, out var period)) return false;

        result = period.Value;
        return true;
    }

    /// <summary>
    /// Formats as ISO 8601 (e.g., "P1Y2M", "P0D").
    /// Supported format specifiers:
    /// - null or "G": default (no transformation)
    /// - "N": normalize months into years
    /// - "W": compress days into weeks if no Y/M and divisible by 7
    /// - "C": canonical (N + W)
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;
        var idx = 0;

        if (destination.Length == 0) return false;
        destination[idx++] = 'P';

        // Local copies so we can transform for formatting without changing state.
        int y = Years, m = Months, w = Weeks, d = Days;

        if (!format.IsEmpty)
        {
            switch (char.ToUpperInvariant(format[0]))
            {
                case 'N':
                    if (m >= 12)
                    {
                        var (yy, mm) = Math.DivRem(m, 12);
                        y += yy;
                        m = mm;
                    }
                    break;

                case 'W':
                    if ((y | m) == 0 && d != 0 && d % 7 == 0)
                    {
                        w += d / 7;
                        d = 0;
                    }
                    break;

                case 'C':
                    if (m >= 12)
                    {
                        var (yy2, mm2) = Math.DivRem(m, 12);
                        y += yy2;
                        m = mm2;
                    }
                    if ((y | m) == 0 && d != 0 && d % 7 == 0)
                    {
                        w += d / 7;
                        d = 0;
                    }
                    break;

                    // 'G' or unknown: treat as default
            }
        }

        if (y != 0)
        {
            if (!y.TryFormat(destination[idx..], out var w1, default, provider)) return false;
            idx += w1;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'Y';
        }
        if (m != 0)
        {
            if (!m.TryFormat(destination[idx..], out var w2, default, provider)) return false;
            idx += w2;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'M';
        }
        if (w != 0)
        {
            if (!w.TryFormat(destination[idx..], out var w3, default, provider)) return false;
            idx += w3;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'W';
        }
        if (d != 0)
        {
            if (!d.TryFormat(destination[idx..], out var w4, default, provider)) return false;
            idx += w4;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'D';
        }

        if (idx == 1) // nothing appended after 'P'
        {
            if (destination.Length < 3) return false; // "P0D"
            destination[idx++] = '0';
            destination[idx++] = 'D';
        }

        charsWritten = idx;
        return true;
    }

    /// <summary>
    /// Formats as ISO 8601 (e.g., "P1Y2M", "P0D").
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

    /// <summary>
    /// Applies this period to a DateOnly, in Y -> M -> D order for date-based periods,
    /// or as 7*W days for week-based periods.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateOnly AddTo(DateOnly date)
    {
        if (Weeks != 0)
        {
            var totalDays = checked(Weeks * 7L);
            return date.AddDays(checked((int)totalDays));
        }

        var result = date;
        if (Years != 0) result = result.AddYears(Years);
        if (Months != 0) result = result.AddMonths(Months);
        if (Days != 0) result = result.AddDays(Days);
        return result;
    }

    /// <summary>
    /// Creates a <see cref="DateAdjuster"/> that applies this period to a DateOnly.
    /// </summary>
    public DateAdjuster ToAddDateAdjuster()
    {
        var period = this; // Copy 'this' to a local variable
        return date => period.AddTo(date);
    }

    /// <summary>
    /// Subtracts this period from a DateOnly, in Y -> M -> D order for date-based periods,
    /// or as 7*W days for week-based periods.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateOnly SubtractFrom(DateOnly date)
    {
        if (Weeks != 0)
        {
            var totalDays = checked(Weeks * 7L);
            return date.AddDays(checked((int)-totalDays));
        }

        var result = date;
        if (Years != 0) result = result.AddYears(-Years);
        if (Months != 0) result = result.AddMonths(-Months);
        if (Days != 0) result = result.AddDays(-Days);
        return result;
    }

    /// <summary>
    /// Creates a <see cref="DateAdjuster"/> that subtracts this period from a DateOnly.
    /// </summary>
    public DateAdjuster ToSubtractDateAdjuster()
    {
        var period = this; // Copy 'this' to a local variable
        return date => period.SubtractFrom(date);
    }
}