namespace B7.Financial.Abstractions.Date;

/// <summary>
/// Provides a factory for creating Period Addition Conventions.
/// </summary>
public interface IPeriodAdditionConventionFactory : INamedFactory<IPeriodAdditionConvention>
{
    /// <summary>
    /// Retrieves the names of all available Period Addition Conventions.
    /// </summary>
    /// <returns> A collection of Period Addition Convention names. </returns>
    public IEnumerable<Name> PeriodAdditionConventionNames();
}