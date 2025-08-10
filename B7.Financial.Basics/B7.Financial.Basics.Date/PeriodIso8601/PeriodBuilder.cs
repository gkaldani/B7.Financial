using B7.Financial.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace B7.Financial.Basics.Date.PeriodIso8601;

/// <summary>
/// Fluent builder for <see cref="Period"/> with optional normalization on build.
/// </summary>
public sealed class PeriodBuilder
{
    private int _years;
    private int _months;
    private int _weeks;
    private int _days;
    private bool _normalizeOnBuild;

    #region Construction

    public PeriodBuilder() { }

    public PeriodBuilder(Period seed)
    {
        _years = seed.Years;
        _months = seed.Months;
        _weeks = seed.Weeks;
        _days  = seed.Days;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PeriodBuilder Create() => new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PeriodBuilder From(Period seed) => new(seed);

    /// <summary>
    /// Parse ISO-8601 text (e.g., "P1Y2M3W4D" or "Zero") and seed the builder.
    /// </summary>
    public static PeriodBuilder From(Name name)
    {
        var p = Period.Parse(name);
        return new PeriodBuilder(p);
    }

    /// <summary>
    /// Try parse ISO-8601 text and seed the builder.
    /// </summary>
    public static bool TryFrom(string value, [NotNullWhen(true)] out PeriodBuilder? builder)
    {
        builder = null;
        if (!Period.TryParse(value, out var p)) return false;
        builder = new PeriodBuilder(p.Value);
        return true;
    }

    #endregion

    #region Fluent "With" setters (replace)

    public PeriodBuilder WithYears(int years)  { _years  = EnsureNonNegative(years, nameof(years));   return this; }
    public PeriodBuilder WithMonths(int months){ _months = EnsureNonNegative(months, nameof(months)); return this; }
    public PeriodBuilder WithWeeks(int weeks)  { _weeks  = EnsureNonNegative(weeks, nameof(weeks));   return this; }
    public PeriodBuilder WithDays(int days)    { _days   = EnsureNonNegative(days, nameof(days));     return this; }

    /// <summary>
    /// Sets years/months from a total months value (e.g., 26 =&gt; 2y 2m).
    /// </summary>
    public PeriodBuilder WithTotalMonths(int totalMonths)
    {
        totalMonths = EnsureNonNegative(totalMonths, nameof(totalMonths));
        var (y, m) = Math.DivRem(totalMonths, 12);
        _years  = y;
        _months = m;
        return this;
    }

    #endregion

    #region Fluent "Add" mutators (increment)

    public PeriodBuilder AddYears(int years)   { _years  = checked(_years  + EnsureNonNegative(years, nameof(years)));   return this; }
    public PeriodBuilder AddMonths(int months) { _months = checked(_months + EnsureNonNegative(months, nameof(months))); return this; }
    public PeriodBuilder AddWeeks(int weeks)   { _weeks  = checked(_weeks  + EnsureNonNegative(weeks, nameof(weeks)));   return this; }
    public PeriodBuilder AddDays(int days)     { _days   = checked(_days   + EnsureNonNegative(days, nameof(days)));     return this; }

    public PeriodBuilder AddTotalMonths(int totalMonths)
    {
        totalMonths = EnsureNonNegative(totalMonths, nameof(totalMonths));
        _months = checked(_months + totalMonths);
        return this;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// When enabled, converts months ≥ 12 to years during <see cref="Build"/>.
    /// </summary>
    public PeriodBuilder Normalize(bool enabled = true)
    {
        _normalizeOnBuild = enabled;
        return this;
    }

    /// <summary>
    /// Resets all components to zero.
    /// </summary>
    public PeriodBuilder Clear()
    {
        _years = _months = _weeks = _days = 0;
        return this;
    }

    /// <summary>
    /// Peek the current state without modifying it.
    /// </summary>
    /// <returns></returns>
    public (int Years, int Months, int Weeks, int Days) Peek() => (_years, _months, _weeks, _days);

    #endregion

    #region Build

    /// <summary>
    /// Create the immutable <see cref="Period"/>. Throws if any part is negative (should never happen).
    /// </summary>
    public Period Build()
    {
        var y = _years;
        var m = _months;

        if (_normalizeOnBuild && m >= 12)
        {
            var (yy, mm) = Math.DivRem(m, 12);
            y = checked(y + yy);
            m = mm;
        }

        // Period ctor enforces non-negativity; we’ve already guarded on set.
        return new Period(y, m, _weeks, _days);
    }

    /// <summary>
    /// Try to create the <see cref="Period"/> without throwing.
    /// </summary>
    public bool TryBuild([NotNullWhen(true)] out Period? period)
    {
        try
        {
            period = Build();
            return true;
        }
        catch
        {
            period = null;
            return false;
        }
    }

    /// <summary>
    /// Implicit conversion for convenience: var p = PeriodBuilder.Create().WithDays(3);
    /// </summary>
    public static implicit operator Period(PeriodBuilder builder) => builder.Build();

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int EnsureNonNegative(int value, string paramName)
        => value >= 0 ? value : throw new ArgumentOutOfRangeException(paramName, "Value must be non-negative.");
}