using System.Collections.Frozen;
using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// Represents a factory for creating standard day count conventions.
/// </summary>
public class StandardDayCountFactory : IDayCountFactory
{
    /// <summary>The '1/1' day count convention.</summary>
    public static readonly DayCountOneOne OneOne = new ();
    /// <summary>The 'Act/Act ISDA' day count convention.</summary>
    public static readonly DayCountActualActualIsda ActualActualIsda = new ();
    /// <summary>The 'Act/365A' day count convention.</summary>
    public static readonly DayCountActual365Actual Actual365Actual = new ();

    private static readonly FrozenDictionary<Name, IDayCount> DayCountConventions =
        new Dictionary<Name, IDayCount>
        {
            [OneOne.Name] = OneOne,
            [ActualActualIsda.Name] = ActualActualIsda,
            [Actual365Actual.Name] = Actual365Actual
        }.ToFrozenDictionary();

    /// <summary>
    /// Retrieves a day count convention by its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>The <see cref="IDayCount"/> instance associated with the specified name.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented and must be overridden in a derived class.</exception>
    public virtual IDayCount Of(Name name) =>
        DayCountConventions.TryGetValue(name, out var convention)
            ? convention
            : throw new NotImplementedException($"Unknown day count convention: {name}");

    /// <inheritdoc />
    public virtual IEnumerable<Name> DayCountNames() => DayCountConventions.Keys;
}