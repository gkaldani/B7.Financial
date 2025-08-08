using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.DayCountConventions;

/// <summary>
/// Represents a factory for creating day count conventions.
/// </summary>
public interface IDayCountFactory : INamedFactory<DayCount>;