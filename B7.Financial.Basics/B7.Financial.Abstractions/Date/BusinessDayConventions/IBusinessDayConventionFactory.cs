namespace B7.Financial.Abstractions.Date.BusinessDayConventions;

/// <summary>
/// Provides a factory for creating business day conventions.
/// </summary>
public interface IBusinessDayConventionFactory : INamedFactory<IBusinessDayConvention>
{
    /// <summary>
    /// Retrieves the names of all available business day conventions.
    /// </summary>
    /// <returns> A collection of business day convention names. </returns>
    public IEnumerable<Name> BusinessDayConventionNames();
}