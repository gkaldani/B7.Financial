using System.Numerics;

namespace B7.Financial.Abstractions.Date;

/// <summary>
/// A tenor indicating how long it will take for a financial instrument to reach maturity.
/// <para>
/// A tenor is allowed to be any non-negative non-zero period of days, weeks, month or years. <br/>
/// </para>
/// <para>
/// Each tenor is based on a <see cref="Period"/>. The months and years of the period are not normalized, <br/>
/// thus it is possible to have a tenor of 12 months and a different one of 1 year. <br/>
/// When used, standard date addition rules apply, thus there is no difference between them. <br/>
/// Call <see cref="ToNormalized"/> to apply normalization. 
/// </para>
/// </summary>
public readonly struct Tenor :
    INamed,
    IEquatable<Tenor>,
    IAdditionOperators<Tenor, Period, Tenor>
{
    public Name Name { get; }
    public bool Equals(Tenor other)
    {
        throw new NotImplementedException();
    }

    public static Tenor operator +(Tenor left, Period right)
    {
        throw new NotImplementedException();
    }

    public Tenor ToNormalized()
    {
        throw new NotImplementedException();
    }
}