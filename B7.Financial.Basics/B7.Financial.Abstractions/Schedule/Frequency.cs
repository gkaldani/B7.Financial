using B7.Financial.Abstractions.Date;

namespace B7.Financial.Abstractions.Schedule;

/// <summary>
/// A periodic frequency used by financial products that have a specific event every so often.
/// <para>
/// Frequency is primarily intended to be used to subdivide events within a year. <br/>
/// A frequency is allowed to be any non-negative period of days, weeks, month or years.
/// A special value, 'Term', is provided for when there are no subdivisions of the entire term. <br/>
/// This is also known as 'zero-coupon' or 'once'.
/// </para>
/// </summary>
public readonly struct Frequency : INamed, IEquatable<Frequency>
{
    private const string TermString = "Term";

    /// <summary>
    /// A periodic frequency matching the term. <br/>
    /// Also known as zero-coupon. <br/>
    /// This is represented using <see cref="Date.Period.Zero"/>. <br/>
    /// There are no events per year with this frequency.
    /// </summary>
    public static readonly Frequency Term = new();

    /// <summary>
    /// Gets the name of the frequency.
    /// </summary>
    public Name Name { get; }

    /// <summary>
    /// Period of the frequency. <br/>
    /// </summary>
    public Period Period { get; }

    /// <summary>
    /// Events per year. <br/>
    /// </summary>
    public int EventsPerYear { get; }

    /// <summary>
    /// Estimated events per year. <br/>
    /// </summary>
    public decimal EventsPerYearEstimate { get; }

    /// <summary>
    /// Constructs a frequency based on the specified period.
    /// </summary>
    /// <param name="period"></param>
    public Frequency(Period period)
    {
        Period = period;

        EventsPerYear = 0;
        EventsPerYearEstimate = 0;

        if (period == Period.Zero)
        {
            Name = TermString;
            return;
        }

        Name = period.ToString();


        var months = period.ToTotalMonths();
        var days = period.Days;

        if (months > 0 && days == 0)
        {
            EventsPerYear = (12 % months == 0) ? 12 / months : -1;
            EventsPerYearEstimate = 12m / months;
        }
        else
        if (days > 0 && months == 0)
        {
            EventsPerYear = (364 % days == 0) ? 364 / days : -1;
            EventsPerYearEstimate = 364m / days;
        }
        else
        {
            EventsPerYear = -1;
            EventsPerYearEstimate = -1;
        }
    }

    /// <summary>
    /// string representation of the frequency.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Name;

    /// <summary>
    /// Indicates if the frequency is the term frequency.
    /// </summary>
    public bool IsTerm => this == Term;

    /// <summary>
    /// Indicates if the periodic frequency is based on weeks.
    /// </summary>
    public bool IsWeekBased => Period.ToTotalMonths() == 0 && Period.Days % 7 == 0;

    /// <summary>
    /// Indicates if the periodic frequency is based on months.
    /// </summary>
    public bool IsMonthBased => Period.ToTotalMonths() > 0 && Period.Days == 0;

    /// <summary>
    /// Checks if the periodic frequency is annual.
    /// <remarks>
    /// An annual frequency consists of 12 months. <br/>
    /// The period must be exactly 12 months with no weeks or days. <br/>
    /// </remarks>
    /// </summary>
    public bool IsAnnual =>
        Period.ToTotalMonths() == 12 && Period is { Weeks: 0, Days: 0 };

    /// <inheritdoc />
    public bool Equals(Frequency other)
    {
        return Name.Equals(other.Name) && Period.Equals(other.Period) && EventsPerYear == other.EventsPerYear && EventsPerYearEstimate == other.EventsPerYearEstimate;  
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Frequency other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Name, Period, EventsPerYear, EventsPerYearEstimate);

    /// <summary>
    /// Compares two frequencies for equality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Frequency left, Frequency right) => left.Equals(right);

    /// <summary>
    /// Compares two frequencies for inequality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Frequency left, Frequency right) => !(left == right);
}