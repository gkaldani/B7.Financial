namespace B7.Financial.Abstractions.Date;

/// <summary>
/// Represents a factory for creating day count conventions.
/// </summary>
public interface IDayCountFactory : INamedFactory<DayCount>
{
    /// <summary>
    /// Retrieves the names of all available day count conventions.
    /// </summary>
    /// <returns> A collection of day count convention names. </returns>
    public IEnumerable<Name> DayCountNames();
}