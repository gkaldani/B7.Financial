using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Schedule;
using System.Collections.Frozen;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.Schedule;

/// <summary>
/// Represents a factory for creating frequency instances.
/// </summary>
public class FrequencyFactory : IFrequencyFactory
{
    /// <summary>
    /// A periodic frequency matching the term. <br/>
    /// Also known as zero-coupon. <br/>
    /// This is represented using the <see cref="Period.MaxValue"/>. <br/>
    /// There are no events per year with this frequency.
    /// </summary>
    public static readonly Frequency Term = Frequency.Term; 

    /// <summary>
    /// A periodic frequency of one day. <br/>
    /// Also known as 'Daily'.
    /// There are considered to be 364 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P1D = new (Period.OfDays(1));

    /// <summary>
    /// A periodic frequency of 1 week (7 days). <br/>
    /// Also known as 'Weekly'.
    /// There are considered to be 52 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P1W = new (Period.OfWeeks(1));

    /// <summary>
    /// A periodic frequency of 2 weeks (14 days). <br/>
    /// Also known as 'Biweekly'.
    /// There are considered to be 26 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P2W = new (Period.OfWeeks(2));

    /// <summary>
    /// A periodic frequency of 4 weeks (28 days). <br/>
    /// Also known as 'Lunar'. <br/>
    /// There are considered to be 13 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P4W = new (Period.OfWeeks(4));

    /// <summary>
    /// A periodic frequency of 13 weeks (91 days). <br/>
    /// There are considered to be 4 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P13W = new (Period.OfWeeks(13));

    /// <summary>
    /// A periodic frequency of 26 weeks (182 days). <br/>
    /// There are considered to be 2 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P26W = new (Period.OfWeeks(26));

    /// <summary>
    /// A periodic frequency of 52 weeks (364 days). <br/>
    /// There are considered to be 1 event per year with this frequency.
    /// </summary>
    public static readonly Frequency P52W = new (Period.OfWeeks(52));

    /// <summary>
    /// A periodic frequency of 1 month. <br/>
    /// Also known as 'Monthly'. <br/>
    /// There are 12 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P1M = new (Period.OfMonths(1));

    /// <summary>
    /// A periodic frequency of 2 months. <br/>
    /// Also known as 'Bi-Monthly'. <br/>
    /// There are 6 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P2M = new (Period.OfMonths(2));

    /// <summary>
    /// A periodic frequency of 3 months. <br/>
    /// Also known as 'Quarterly'. <br/>
    /// There are 4 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P3M = new (Period.OfMonths(3));

    /// <summary>
    /// A periodic frequency of 4 months. <br/>
    /// There are 3 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P4M = new (Period.OfMonths(4));

    /// <summary>
    /// A periodic frequency of 6 months. <br/>
    /// Also known as 'Semi-Annual'. <br/>
    /// There are 2 events per year with this frequency.
    /// </summary>
    public static readonly Frequency P6M = new (Period.OfMonths(6));

    /// <summary>
    /// A periodic frequency of 12 months (1 year). <br/>
    /// Also known as 'Annual'.
    /// There is 1 event per year with this frequency.
    /// </summary>
    public static readonly Frequency P12M = new (Period.OfMonths(12));

    /// <summary>
    /// A periodic frequency of 1 year. <br/>
    /// Also known as 'Annual'.
    /// There is 1 event per year with this frequency.
    /// </summary>
    public static readonly Frequency P1Y = new (Period.OfYears(1));

    /// <inheritdoc />
    public static Frequency OfDays(int days) => new (Period.OfDays(days));

    /// <inheritdoc />
    public static Frequency OfWeeks(int weeks) =>
     weeks switch
     {
         1 => P1W,
         2 => P2W,
         4 => P4W,
         13 => P13W,
         26 => P26W,
         52 => P52W,
         _ => new Frequency(Period.OfWeeks(weeks))
     };

    /// <inheritdoc />
    public static Frequency OfMonths(int months) =>
        months switch
        {
            1 => P1M,
            2 => P2M,
            3 => P3M,
            4 => P4M,
            6 => P6M,
            12 => P12M,
            _ => new Frequency(Period.OfMonths(months))
        };

    /// <inheritdoc />
    public static Frequency OfYears(int years) => new (Period.OfYears(years));

    /// <summary>
    /// Read-only dictionary containing standard predefined frequencies for fast lookup.
    /// </summary>
    private static readonly FrozenDictionary<Name, Frequency> PredefinedFrequencies =
        new Dictionary<Name, Frequency>
        {
            //Term and standard frequencies
            [Term.Name] = Term,

            //Daily frequencies
            [P1D.Name] = P1D,

            //Weekly frequencies
            [P1W.Name] = P1W, [P2W.Name] = P2W, [P4W.Name] = P4W, [P13W.Name] = P13W, [P26W.Name] = P26W, [P52W.Name] = P52W,

            // Monthly frequencies
            [P1M.Name] = P1M, [P2M.Name] = P2M, [P3M.Name] = P3M, [P4M.Name] = P4M, [P6M.Name] = P6M, [P12M.Name] = P12M

        }.ToFrozenDictionary();

    /// <summary>
    /// Retrieves a frequency by its name.
    /// </summary>
    /// <param name="name">The name of the frequency to retrieve.</param>
    /// <returns>
    /// The frequency instance corresponding to the specified name. 
    /// Returns a standard frequency if found in the predefined collection, 
    /// otherwise parses and returns a new frequency instance.
    /// </returns>
    public virtual Frequency Of(Name name) =>
        PredefinedFrequencies.TryGetValue(name, out var frequency)
            ? frequency
            : Frequency.Parse(name);

    /// <inheritdoc />
    public virtual IEnumerable<Name> FrequencyNames() => PredefinedFrequencies.Keys;
}