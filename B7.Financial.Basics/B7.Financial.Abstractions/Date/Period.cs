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
    public static Period MaxValue { get; } = new(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

    /// <summary>
    /// Minimum value for <see cref="Period"/>.
    /// </summary>
    public static Period MinValue { get; } = Zero;

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified days.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfDays(int days) => new(years: 0, months: 0, weeks: 0, days: days);

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified months.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfMonths(int months) => new(years: 0, months: months, weeks: 0, days: 0);

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified weeks.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfWeeks(int weeks) => new(years: 0, months: 0, weeks: weeks, days: 0);

    /// <summary>
    /// Creates a new instance of <see cref="Period"/> with the specified years.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Period OfYears(int years) => new(years: years, months: 0, weeks: 0, days: 0);

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
    /// Constructs a new instance of <see cref="Period"/> with the specified values.
    /// </summary>
    /// <exception cref="ArgumentException">All values must be non-negative.</exception>
    public Period(int years, int months, int weeks, int days)
    {
        // Single validation check with bitwise OR for better performance
        if ((years | months | weeks | days) < 0)
            throw new ArgumentException("All values must be non-negative.");

        Years = years;
        Months = months;
        Weeks = weeks;
        Days = days;
    }

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
                        anyComponent = true;
                        break;
                    case 'M':
                        if (months != 0) return false; // Duplicate
                        if (hasWeeksComponent) return false;
                        months = number;
                        anyComponent = true;
                        break;
                    case 'W':
                        if ((years | months | days) != 0) return false; // ISO: W cannot mix with Y/M/D
                        if (weeks != 0) return false; // Duplicate
                        weeks = number;
                        hasWeeksComponent = true;
                        anyComponent = true;
                        break;
                    case 'D':
                        if (days != 0) return false; // Duplicate
                        if (hasWeeksComponent) return false;
                        days = number;
                        anyComponent = true;
                        break;
                    default:
                        return false;
                }

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
    /// </summary>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        charsWritten = 0;
        var idx = 0;

        if (destination.Length == 0) return false;
        destination[idx++] = 'P';

        if (Years != 0)
        {
            if (!Years.TryFormat(destination[idx..], out var w, default, provider)) return false;
            idx += w;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'Y';
        }
        if (Months != 0)
        {
            if (!Months.TryFormat(destination[idx..], out var w, default, provider)) return false;
            idx += w;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'M';
        }
        if (Weeks != 0)
        {
            if (!Weeks.TryFormat(destination[idx..], out var w, default, provider)) return false;
            idx += w;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'W';
        }
        if (Days != 0)
        {
            if (!Days.TryFormat(destination[idx..], out var w, default, provider)) return false;
            idx += w;
            if (idx >= destination.Length) return false;
            destination[idx++] = 'D';
        }

        if (idx == 1) // nothing appended after 'P'
        {
            if (destination.Length < 4) return false; // "P0D"
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
}