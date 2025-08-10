using System.Collections.Frozen;
using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// Represents a factory for creating standard day count conventions.
/// </summary>
public class StandardDayCountsFactory : IDayCountFactory
{
    public static readonly DayCountOneOne OneOne = new ();
    public static readonly DayCountActualActualIsda ActualActualIsda = new ();

    private static readonly FrozenDictionary<Name, DayCount> DayCountConventions =
        new Dictionary<Name, DayCount>
        {
            [OneOne.Name] = OneOne,
            [ActualActualIsda.Name] = ActualActualIsda,
        }.ToFrozenDictionary();

    /// <summary>
    /// Retrieves a day count convention by its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public virtual DayCount Of(Name name) =>
        DayCountConventions.TryGetValue(name, out var convention)
            ? convention
            : throw new ArgumentException($"Unknown day count convention: {name}");

    public virtual IEnumerable<Name> DayCountNames() => DayCountConventions.Keys;
}