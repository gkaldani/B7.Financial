using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using System.Collections.Frozen;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// Represents a factory for creating standard business day conventions.
/// </summary>
public class StandardBusinessDayConventionFactory : IBusinessDayConventionFactory
{
    /// <summary> Make no adjustment </summary>
    public static readonly IBusinessDayConvention NoAdjust = BusinessDayNoAdjust.NoAdjust;

    /// <summary> Next business day </summary>
    public static readonly BusinessDayFollowing Following = new();

    /// <summary> Next business day unless over a month end </summary>
    public static readonly BusinessDayModifiedFollowing ModifiedFollowing = new();

    /// <summary> Next business day unless over a month end or mid </summary>
    public static readonly BusinessDayModifiedFollowingBiMonthly ModifiedFollowingBiMonthly = new();

    /// <summary> Previous business day </summary>
    public static readonly BusinessDayPreceding Preceding = new();

    /// <summary> Previous business day unless over a month end </summary>
    public static readonly BusinessDayModifiedPreceding ModifiedPreceding = new();

    /// <summary> Next business day if Sun/Mon, otherwise previous </summary>
    public static readonly BusinessDayNearest Nearest = new();

    private static readonly FrozenDictionary<Name, IBusinessDayConvention> BusinessDayConventions =
        new Dictionary<Name, IBusinessDayConvention>
        {
            [NoAdjust.Name] = NoAdjust,

            [Following.Name] = Following,
            [ModifiedFollowing.Name] = ModifiedFollowing,
            [ModifiedFollowingBiMonthly.Name] = ModifiedFollowingBiMonthly,

            [Preceding.Name] = Preceding,
            [ModifiedPreceding.Name] = ModifiedPreceding,

            [Nearest.Name] = Nearest

        }.ToFrozenDictionary();

    /// <summary>
    /// Retrieves the business day convention associated with the specified name.
    /// </summary>
    /// <param name="name">The name representing the desired business day convention.</param>
    /// <returns>An instance of <see cref="IBusinessDayConvention"/> corresponding to the specified name.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented and must be overridden in a derived class.</exception>
    public virtual IBusinessDayConvention Of(Name name) =>
        BusinessDayConventions.TryGetValue(name, out var convention)
            ? convention 
            : throw new NotImplementedException($"Unknown business day convention: {name}");

    /// <inheritdoc />
    public virtual IEnumerable<Name> BusinessDayConventionNames() => BusinessDayConventions.Keys;
}