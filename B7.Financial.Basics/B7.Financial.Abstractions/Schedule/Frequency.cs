using B7.Financial.Abstractions.Date;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace B7.Financial.Abstractions.Schedule;

/// <summary>
/// A periodic frequency used by financial products that have a specific event every so often.
/// <para>
/// Frequency is primarily intended to be used to subdivide events within a year. <br/>
/// A frequency is allowed to be any non-negative <see cref="Date.Period"/>. <br/>
/// A special value, <see cref="Frequency.Term"/>, is provided for when there are no subdivisions of the entire term. <br/>
/// This is also known as 'zero-coupon' or 'once'.
/// </para>
/// </summary>
public readonly struct Frequency :
    INamed,
    IEquatable<Frequency>,
    IAdditionOperators<Frequency, Period, Frequency>
{
    private const string TermString = "Term";

    /// <summary>
    /// A periodic frequency matching the term. <br/>
    /// Also known as zero-coupon. <br/>
    /// This is represented using <see cref="Date.Period.MaxValue"/>. <br/>
    /// There are no events per year with this frequency.
    /// </summary>
    public static readonly Frequency Term = new(Period.MaxValue);

    /// <summary>
    /// Gets the name of the frequency. <br/>
    /// For the term frequency this is "Term", otherwise the ISO 8601 period text.
    /// </summary>
    public Name Name { get; }

    /// <summary>
    /// Period of the frequency. <br/>
    /// </summary>
    public Period Period => _period ?? throw new InvalidOperationException("Frequency is not initialized.");

    // Backing field so default(Frequency) is safe.
    private readonly Period? _period;

    /// <summary>
    /// Exact whole number of events per (synthetic) year, or -1 if not an even divisor
    /// of 12 months (month-based) or 364 days (day/week-based).
    /// </summary>
    public int EventsPerYear { get; }

    /// <summary>
    /// Estimated events per year (decimal). -1 when not computable under current rules.
    /// </summary>
    public decimal EventsPerYearEstimate { get; }

    /// <summary>
    /// Indicates this frequency has an exact integral events-per-year value (EventsPerYear > 0).
    /// </summary>
    public bool HasExactEventsPerYear => EventsPerYear > 0;

    /// <summary>
    /// Alias for <see cref="HasExactEventsPerYear"/> (semantic clarity in some domains).
    /// </summary>
    public bool IsRegular => HasExactEventsPerYear;

    /// <summary>
    /// Constructs a frequency based on the specified period.
    /// </summary>
    /// <param name="period">The period for this frequency.</param>
    public Frequency(Period period)
    {
        if (period == Period.Zero)
            throw new ArgumentException("Period cannot be zero.", nameof(period));

        _period = period;

        if (period == Period.MaxValue)
        {
            Name = TermString;
            EventsPerYear = 0;
            EventsPerYearEstimate = 0;
            return;
        }

        Name = period.ToString();

        (EventsPerYear, EventsPerYearEstimate) = ComputeEventsPerYear(period);
    }

    /// <summary>
    /// Parses a frequency from its string representation.
    /// </summary>
    /// <param name="value">The string representation (either "Term" or ISO 8601 period).</param>
    /// <returns>The parsed frequency.</returns>
    /// <exception cref="ArgumentException">Thrown when the value cannot be parsed.</exception>
    public static Frequency Parse(Name value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));

        if (value.Value.Equals(TermString, StringComparison.OrdinalIgnoreCase))
            return Term;

        if (Period.TryParse(value, out var period))
            return new Frequency(period.Value);

        throw new ArgumentException($"Cannot parse '{value}' as a frequency.", nameof(value));
    }

    /// <summary>
    /// Tries to parse a frequency from its string representation.
    /// </summary>
    /// <param name="value">The string representation.</param>
    /// <param name="frequency">The parsed frequency if successful.</param>
    /// <returns>True if parsing succeeded, false otherwise.</returns>
    public static bool TryParse(Name value, [NotNullWhen(true)]out Frequency? frequency)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            frequency = null;
            return false;
        }

        if (value.Value.Equals(TermString, StringComparison.OrdinalIgnoreCase))
        {
            frequency = Term;
            return true;
        }

        if (Period.TryParse(value, out var period))
        {
            frequency = new Frequency(period.Value);
            return true;
        }

        frequency = null;
        return false;
    }

    private static (int exact, decimal estimate) ComputeEventsPerYear(Period period)
    {
        // Fast reject mixed components that can't be standardized.
        var months = period.ToTotalMonths();
        var days = period.Days;

        // (Weeks already excluded from month-based path; day logic still OK if weeks==0; week-based: treat via days path)
        if (months > 0 && days == 0 && period.Weeks == 0)
        {
            var exact = (12 % months == 0) ? 12 / months : -1;
            var estimate = (decimal)12 / months;
            return (exact, estimate);
        }

        // Day / week based (weeks converted externally when Period was built)
        if (days > 0 && months == 0 && period.Weeks == 0)
        {
            var exact = (364 % days == 0) ? 364 / days : -1;
            var estimate = (decimal)364 / days;
            return (exact, estimate);
        }

        // Week-based (pure weeks); compute using 52 weeks baseline.
        if (period is { IsWeekBased: true, Weeks: > 0 })
        {
            var weeks = period.Weeks;
            var exact = (52 % weeks == 0) ? 52 / weeks : -1;
            var estimate = 52m / weeks;
            return (exact, estimate);
        }

        return (-1, -1);
    }

    /// <summary>
    /// Attempts to construct a frequency from an integer events-per-year value.
    /// Supports:
    /// - Month-based divisors of 12 (e.g. 12, 6, 4, 3, 2, 1)
    /// - Week-based divisors of 52
    /// - Day-based divisors of 364
    /// Returns false if no exact canonical period can be produced.
    /// </summary>
    public static bool TryFromEventsPerYear(int eventsPerYear, [NotNullWhen(true)] out Frequency? frequency)
    {
        if (eventsPerYear <= 0)
        {
            frequency = Term;
            return eventsPerYear == 0;  // Return true only for exactly zero
        }

        // Prefer month-based mapping for common financial frequencies
        if (12 % eventsPerYear == 0)
        {
            var months = 12 / eventsPerYear;
            frequency = new Frequency(Period.OfMonths(months));
            return true;
        }

        // Week-based exact mapping
        if (52 % eventsPerYear == 0)
        {
            var weeks = 52 / eventsPerYear;
            frequency = new Frequency(Period.OfWeeks(weeks));
            return true;
        }

        // Day-based exact mapping over 364-day synthetic base
        if (364 % eventsPerYear == 0)
        {
            var days = 364 / eventsPerYear;
            frequency = new Frequency(Period.OfDays(days));
            return true;
        }

        frequency = null;
        return false;
    }

    /// <summary>
    /// Creates a frequency from events per year, throwing an exception if not possible.
    /// </summary>
    /// <param name="eventsPerYear">Number of events per year.</param>
    /// <returns>A frequency instance.</returns>
    /// <exception cref="ArgumentException">Thrown when eventsPerYear cannot be represented as an exact frequency.</exception>
    public static Frequency FromEventsPerYear(int eventsPerYear)
    {
        if (TryFromEventsPerYear(eventsPerYear, out var frequency))
            return frequency.Value;
        
        throw new ArgumentException($"Cannot create an exact frequency for {eventsPerYear} events per year.", nameof(eventsPerYear));
    }

    /// <summary>
    /// Deconstructs the frequency into its core components.
    /// </summary>
    public void Deconstruct(out Period period, out int eventsPerYear, out decimal eventsPerYearEstimate)
    {
        period = Period;
        eventsPerYear = EventsPerYear;
        eventsPerYearEstimate = EventsPerYearEstimate;
    }

    /// <summary>
    /// String representation of the frequency.
    /// </summary>
    /// <returns>The frequency name.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Indicates if the frequency is the term frequency.
    /// </summary>
    public bool IsTerm => Period == Period.MaxValue;

    /// <summary>
    /// Indicates if the periodic frequency is based on weeks.
    /// </summary>
    public bool IsWeekBased => Period.IsWeekBased;

    /// <summary>
    /// Indicates if the periodic frequency is based on months.
    /// </summary>
    public bool IsMonthBased => Period.IsDateBased && Period.ToTotalMonths() > 0 && Period.Days == 0;

    /// <summary>
    /// Indicates if the periodic frequency is based on days.
    /// </summary>
    public bool IsDayBased => Period.IsDateBased && Period.ToTotalMonths() == 0 && Period.Days > 0;

    /// <summary>
    /// Checks if the periodic frequency is annual.
    /// <remarks>
    /// An annual frequency consists of 12 months. <br/>
    /// The period must be exactly 12 months with no weeks or days. <br/>
    /// </remarks>
    /// </summary>
    public bool IsAnnual => Period.ToTotalMonths() == 12 && Period is { Weeks: 0, Days: 0 };

    /// <summary>
    /// Checks if the periodic frequency is quarterly (3 months).
    /// </summary>
    public bool IsQuarterly => Period.ToTotalMonths() == 3 && Period is { Weeks: 0, Days: 0 };

    /// <summary>
    /// Checks if the periodic frequency is monthly (1 month).
    /// </summary>
    public bool IsMonthly => Period.ToTotalMonths() == 1 && Period is { Weeks: 0, Days: 0 };

    /// <summary>
    /// Returns the reciprocal frequency if this frequency has exact events per year.
    /// For example, Monthly (12 events) returns Annual (1 event), Quarterly (4 events) returns Annual, etc.
    /// </summary>
    /// <returns>The reciprocal frequency.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the frequency doesn't have exact events per year.</exception>
    public Frequency ToReciprocal()
    {
        if (!HasExactEventsPerYear)
            throw new InvalidOperationException("Cannot compute reciprocal of a frequency without exact events per year.");

        if (IsTerm)
            throw new InvalidOperationException("Cannot compute reciprocal of Term frequency.");

        // For exact frequencies, reciprocal means creating a frequency where this frequency's 
        // EventsPerYear becomes the period multiplier
        if (IsMonthBased)
        {
            return new Frequency(Period.OfMonths(EventsPerYear));
        }
    
        if (IsWeekBased)
        {
            return new Frequency(Period.OfWeeks(EventsPerYear));
        }
    
        if (IsDayBased)
        {
            return new Frequency(Period.OfDays(EventsPerYear));
        }

        throw new InvalidOperationException("Cannot compute reciprocal for this frequency type.");
    }

    /// <summary>
    /// Attempts to get the reciprocal frequency.
    /// </summary>
    /// <param name="reciprocal">The reciprocal frequency if successful.</param>
    /// <returns>True if reciprocal could be computed, false otherwise.</returns>
    public bool TryGetReciprocal(out Frequency reciprocal)
    {
        try
        {
            reciprocal = ToReciprocal();
            return true;
        }
        catch (InvalidOperationException)
        {
            reciprocal = default;
            return false;
        }
    }

    /// <summary>
    /// Determines if this frequency is compatible with the specified frequency.
    /// Frequencies are compatible if they have the same base type (day/week/month).
    /// </summary>
    /// <param name="other">The frequency to check against.</param>
    /// <returns>True if the frequencies are compatible.</returns>
    public bool IsCompatibleWith(Frequency other)
    {
        if (IsTerm || other.IsTerm)
            return true;
        
        if (IsMonthBased && other.IsMonthBased)
            return true;
        
        if (IsWeekBased && other.IsWeekBased)
            return true;
        
        if (IsDayBased && other.IsDayBased)
            return true;
        
        return false;
    }

    /// <inheritdoc />
    public bool Equals(Frequency other) => Period.Equals(other.Period);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Frequency other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Period.GetHashCode();

    /// <summary>
    /// Compares two frequencies for equality.
    /// </summary>
    public static bool operator ==(Frequency left, Frequency right) => left.Equals(right);

    /// <summary>
    /// Compares two frequencies for inequality.
    /// </summary>
    public static bool operator !=(Frequency left, Frequency right) => !(left == right);

    /// <summary>
    /// Normalizes the months and years of this tenor.
    /// <para>
    /// This method returns a tenor of an equivalent length but with any number <br/>
    /// of months greater than 12 normalized into a combination of months and years.
    /// </para>
    /// </summary>
    /// <returns>The normalized tenor</returns>
    public Frequency ToNormalized() => 
        IsTerm ? Term : new Frequency(Period.ToNormalized());

    /// <summary>
    /// Adds a period to a frequency.
    /// </summary>
    public static Frequency operator +(Frequency left, Period right)
    {
        if (left.IsTerm)
            return new Frequency(right);
        // Add the period to the existing frequency's period
        var newPeriod = left.Period + right;
        return new Frequency(newPeriod);
    }

    /// <summary>
    /// Adds the current frequency to the specified date and returns the resulting date.
    /// </summary>
    /// <param name="date">The starting date to which the frequency will be added.</param>
    /// <returns>A <see cref="DateOnly"/> representing the resulting date after adding the frequency.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the frequency represents a term and cannot be added to a date.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateOnly AddTo(DateOnly date)
    {
        if (IsTerm)
            throw new InvalidOperationException("Cannot add to a term frequency.");

        return Period.AddTo(date);
    }

    /// <summary>
    /// Creates a <see cref="DateAdjuster"/> that adds the current frequency to a date.
    /// </summary>
    public DateAdjuster ToAddDateAdjuster()
    {
        if (IsTerm)
            throw new InvalidOperationException("Cannot create a date adjuster from a term frequency.");
        var frequency = this; // Capture the frequency in a closure
        return date =>  frequency.AddTo(date);
    }

    /// <summary>
    /// Subtracts the current frequency from the specified date and returns the resulting date.
    /// </summary>
    /// <param name="date">The starting date from which the frequency will be subtracted.</param>
    /// <returns>A <see cref="DateOnly"/> representing the resulting date after subtracting the frequency.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the frequency represents a term and cannot be subtracted from a date.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateOnly SubtractFrom(DateOnly date)
    {
        if (IsTerm)
            throw new InvalidOperationException("Cannot subtract from a term frequency.");

        return Period.SubtractFrom(date);
    }

    /// <summary>
    /// Creates a <see cref="DateAdjuster"/> that subtracts the current frequency from a date.
    /// </summary>
    public DateAdjuster ToSubtractDateAdjuster()
    {
        if (IsTerm)
            throw new InvalidOperationException("Cannot create a date adjuster from a term frequency.");
        var frequency = this; // Capture the frequency in a closure
        return date => frequency.SubtractFrom(date);
    }

    /// <summary>
    /// Attempts to compute an exact integer factor n such that:
    ///     this = subFrequency * n
    /// for compatible base types (month-based, week-based, or day-based).
    /// Returns false when:
    /// - Either frequency is Term
    /// - Bases differ (month vs week vs day)
    /// - Periods are irregular mixed Y/M/D (contain both months>0 and days>0) 
    /// - Division is not exact
    /// </summary>
    /// <param name="subFrequency">Candidate sub-frequency (must be same or smaller)</param>
    /// <param name="factor">Resulting integer factor (>=1) when successful.</param>
    public bool TryExactDivide(Frequency subFrequency, out int factor)
    {
        factor = 0;

        // Term cannot participate (ambiguous length)
        if (IsTerm || subFrequency.IsTerm)
            return false;

        // Determine base compatibility (reuse existing semantics).
        var sameBase =
            (IsMonthBased && subFrequency.IsMonthBased) ||
            (IsWeekBased && subFrequency.IsWeekBased) ||
            (IsDayBased && subFrequency.IsDayBased);

        if (!sameBase)
            return false;

        // Guard against irregular mixed Y/M/D periods (months>0 && days>0) which we
        // currently do not treat as either pure month-based or day-based for divisibility.
        // (IsMonthBased / IsDayBased were already false in that scenario, so we'd have exited,
        // but keep future-proof logic explicit.)
        if (!IsMonthBased && !IsWeekBased && !IsDayBased)
            return false;
        if (subFrequency is { IsMonthBased: false, IsWeekBased: false, IsDayBased: false })
            return false;

        int numerator;
        int denominator;

        if (IsMonthBased) // Use total months (years normalized).
        {
            numerator   = Period.ToTotalMonths();
            denominator = subFrequency.Period.ToTotalMonths();
        }
        else if (IsWeekBased)   // Use weeks.
        {
            numerator   = Period.Weeks;
            denominator = subFrequency.Period.Weeks;
        }
        else                    // Day-based.
        {
            numerator   = Period.Days;
            denominator = subFrequency.Period.Days;
        }

        // Basic sanity.
        if (denominator <= 0)
            return false;

        // subFrequency must not exceed this.
        if (numerator < denominator)
            return false;

        if (numerator % denominator != 0)
            return false;

        factor = numerator / denominator;
        return factor >= 1;
    }

    /// <summary>
    /// Computes an exact integer factor n such that:
    ///     this = subFrequency * n
    /// for compatible base types (month / week / day).
    /// Throws when:
    /// - Not exact
    /// - Bases differ
    /// - Term involved
    /// - Irregular unsupported period shapes
    /// </summary>
    /// <param name="subFrequency">Candidate sub-frequency (same base, not larger).</param>
    /// <returns>Integer factor (>=1).</returns>
    /// <exception cref="InvalidOperationException">Division not exact or frequencies incompatible.</exception>
    public int ExactDivide(Frequency subFrequency)
    {
        // Handle common error cases with specific messages for better diagnostics
        if (IsTerm || subFrequency.IsTerm)
            throw new InvalidOperationException("Cannot divide when Term frequency is involved.");

        // Check base type compatibility explicitly
        if (!IsCompatibleWith(subFrequency))
            throw new InvalidOperationException($"Cannot divide frequencies with incompatible base types: '{this}' and '{subFrequency}'.");

        if (!TryExactDivide(subFrequency, out var factor))
            throw new InvalidOperationException($"Frequency '{subFrequency}' does not exactly divide '{this}'.");
        return factor;
    }
}