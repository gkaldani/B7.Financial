using System.Collections.Frozen;
using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.PeriodAdditionConventions;

/// <summary>
/// Represents a factory for creating standard period addition conventions.
/// </summary>
public class StandardPeriodAdditionConventionFactory : IPeriodAdditionConventionFactory
{
    /// <summary> no specific addition rule </summary>
    public static readonly PeriodAdditionNone None = new();

    /// <summary> last day of month addition rule </summary>
    public static readonly PeriodAdditionLastDay LastDay = new();

    private static readonly FrozenDictionary<Name, IPeriodAdditionConvention> PeriodAdditionConventions =
        new Dictionary<Name, IPeriodAdditionConvention>
        {
            [None.Name] = None,
            [LastDay.Name] = LastDay
        }.ToFrozenDictionary();

    /// <inheritdoc />
    public IPeriodAdditionConvention Of(Name name) =>
        PeriodAdditionConventions.TryGetValue(name, out var convention) 
            ? convention 
            : throw new ArgumentOutOfRangeException(nameof(name), name, null);

    /// <inheritdoc />
    public IEnumerable<Name> PeriodAdditionConventionNames() => PeriodAdditionConventions.Keys;
}