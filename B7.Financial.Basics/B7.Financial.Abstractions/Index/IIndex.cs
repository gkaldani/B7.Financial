namespace B7.Financial.Abstractions.Index;

/// <summary>
/// An index of values, such as LIBOR, FED FUND or daily exchange rates.
/// <para>
/// An index is an agreed mechanism for determining certain financial indicators, <br/>
/// such as exchange rate or interest rates. Most common indices are daily.
/// </para>
/// </summary>
public interface IIndex : INamed;
