using System.Globalization;

using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.Tests;

public sealed class PeriodTests
{
    [Fact]
    public void StaticProperties_AreDefinedAndConsistent()
    {
        Assert.Equal(Period.Zero, Period.AdditiveIdentity);
        Assert.Equal(default, Period.Zero);
        Assert.Equal(new Period(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue), Period.MaxValue);
        Assert.Equal(Period.Zero, Period.MinValue);
    }

    [Fact]
    public void Constructor_Throws_OnNegativeValues()
    {
        Assert.Throws<ArgumentException>(() => new Period(-1, 0, 0, 0));
        Assert.Throws<ArgumentException>(() => new Period(0, -1, 0, 0));
        Assert.Throws<ArgumentException>(() => new Period(0, 0, -1, 0));
        Assert.Throws<ArgumentException>(() => new Period(0, 0, 0, -1));
    }

    [Fact]
    public void FactoryMethods_CreateExpectedComponents()
    {
        var d = Period.OfDays(10);
        Assert.Equal(0, d.Years);
        Assert.Equal(0, d.Months);
        Assert.Equal(0, d.Weeks);
        Assert.Equal(10, d.Days);

        var w = Period.OfWeeks(3);
        Assert.Equal(0, w.Years);
        Assert.Equal(0, w.Months);
        Assert.Equal(3, w.Weeks);
        Assert.Equal(0, w.Days);

        var m = Period.OfMonths(6);
        Assert.Equal(0, m.Years);
        Assert.Equal(6, m.Months);
        Assert.Equal(0, m.Weeks);
        Assert.Equal(0, m.Days);

        var y = Period.OfYears(2);
        Assert.Equal(2, y.Years);
        Assert.Equal(0, y.Months);
        Assert.Equal(0, y.Weeks);
        Assert.Equal(0, y.Days);
    }

    [Fact]
    public void Properties_IsZero_IsNormalized()
    {
        Assert.True(Period.Zero.IsZero);
        Assert.True(Period.Zero.IsNormalized);

        var p = new Period(1, 0, 0, 0);
        Assert.False(p.IsZero);
        Assert.True(p.IsNormalized);

        var notNormalized = new Period(0, 15, 0, 0);
        Assert.False(notNormalized.IsNormalized);
    }

    [Fact]
    public void Name_Property_Mirrors_ToString()
    {
        var p = new Period(1, 2, 3, 4);
        Assert.Equal(p.ToString(), p.Name.ToString());
    }

    [Fact]
    public void Equals_And_GetHashCode()
    {
        var a = new Period(1, 2, 3, 4);
        var b = new Period(1, 2, 3, 4);
        var c = new Period(1, 2, 3, 5);

        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());

        Assert.False(a.Equals(c));
        Assert.False(a.Equals((object)c));
    }

    [Fact]
    public void Relational_And_Equality_Operators()
    {
        var a = new Period(0, 12, 0, 0);
        var b = new Period(1, 0, 0, 0);

        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Addition_NormalizesMonths_AndIsChecked()
    {
        var a = new Period(1, 10, 3, 4);
        var b = new Period(2, 5, 1, 6);
        var sum = a + b;

        // months: 10 + 5 = 15 => +1 year, 3 months
        Assert.Equal(1 + 2 + 1, sum.Years);
        Assert.Equal(3, sum.Months);
        Assert.Equal(3 + 1, sum.Weeks);
        Assert.Equal(4 + 6, sum.Days);

        // checked overflow on components (days easiest to trigger)
        var maxDays = Period.OfDays(int.MaxValue);
        Assert.Throws<OverflowException>(() => { var _ = maxDays + Period.OfDays(1); });

        // months overflow before normalization should still throw when sum exceeds int.MaxValue
        var nearMaxMonths = new Period(0, int.MaxValue, 0, 0);
        Assert.Throws<OverflowException>(() => { var _ = nearMaxMonths + Period.OfMonths(1); });
    }

    [Fact]
    public void Multiply_NormalizesMonths_ZeroAndExceptions()
    {
        var p = new Period(0, 14, 2, 3); // 14M => 1Y 2M
        var times2 = p * 2;

        // months: 14 * 2 = 28 => +2Y 4M
        // years: 0 * 2 + 2 = 2; total years 2 + 2 = 4
        Assert.Equal(2, times2.Years);
        Assert.Equal(4, times2.Months);
        Assert.Equal(4, times2.Weeks);
        Assert.Equal(6, times2.Days);

        // multiplying by 0 yields Zero
        Assert.Equal(Period.Zero, p * 0);

        // negative factor throws
        Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = p * -1; });

        // checked overflow
        var maxYears = new Period(int.MaxValue, 0, 0, 0);
        Assert.Throws<OverflowException>(() => { var _ = maxYears * 2; });
    }

    [Fact]
    public void ToString_And_Format_Overloads()
    {
        Assert.Equal("P0D", Period.Zero.ToString());
        Assert.Equal("P1Y2M3W4D", new Period(1, 2, 3, 4).ToString());

        // The IFormattable-style overload funnels to parameterless ToString
        var p = new Period(0, 5, 0, 0);
        Assert.Equal(p.ToString(), p.ToString("G", CultureInfo.InvariantCulture));
        Assert.Equal(p.ToString(), p.ToString(null, null));
    }

    [Fact]
    public void TryFormat_Succeeds_WhenBufferLargeEnough()
    {
        var p = new Period(1, 2, 3, 4);
        var expected = p.ToString();

        var buffer = new char[expected.Length];
        var ok = p.TryFormat(buffer, out var written, default, provider: null);

        Assert.True(ok);
        Assert.Equal(expected.Length, written);
        Assert.Equal(expected, new string(buffer.AsSpan(0, written)));
    }

    [Fact]
    public void TryFormat_Fails_WhenBufferTooSmall()
    {
        var p = new Period(1, 2, 3, 4);
        var expected = p.ToString();

        // One less than needed should fail
        var small = new char[Math.Max(0, expected.Length - 1)];
        var ok = p.TryFormat(small, out var written, default, provider: null);
        Assert.False(ok);
        Assert.Equal(0, written);

        // For Zero, implementation requires length >= 4 (stricter than "P0D" length 3)
        var zeroBuf = new char[3];
        var zeroOk = Period.Zero.TryFormat(zeroBuf, out var zeroWritten, default, provider: null);
        Assert.False(zeroOk);
        Assert.Equal(0, zeroWritten);
    }

    [Fact]
    public void ToTotalMonths_ComputesCorrectly()
    {
        Assert.Equal(0, Period.Zero.ToTotalMonths());
        Assert.Equal(12, new Period(1, 0, 0, 0).ToTotalMonths());
        Assert.Equal(14, new Period(1, 2, 0, 0).ToTotalMonths());
        Assert.Equal(5, new Period(0, 5, 7, 10).ToTotalMonths()); // weeks/days ignored
    }

    [Fact]
    public void ToNormalized_ConvertsMonthsToYears_WhenNeeded()
    {
        var already = new Period(2, 5, 1, 1);
        Assert.Equal(already, already.ToNormalized());

        var notNorm = new Period(1, 26, 2, 3); // 26M => +2Y 2M
        var norm = notNorm.ToNormalized();
        Assert.Equal(1 + 2, norm.Years);
        Assert.Equal(2, norm.Months);
        Assert.Equal(2, norm.Weeks);
        Assert.Equal(3, norm.Days);
        Assert.True(norm.IsNormalized);
    }

    [Fact]
    public void Deconstruct_ProvidesAllComponents()
    {
        var p = new Period(3, 4, 5, 6);
        p.Deconstruct(out var y, out var m, out var w, out var d);

        Assert.Equal(3, y);
        Assert.Equal(4, m);
        Assert.Equal(5, w);
        Assert.Equal(6, d);
    }

    [Fact]
    public void Parse_Name_Overload_Succeeds_And_InvalidThrows()
    {
        var p = Period.Parse(new Name("P1Y2M4D"));
        Assert.Equal(new Period(1, 2, 0, 4), p);

        Assert.Throws<FormatException>(() => Period.Parse(new Name("P")));
        Assert.Throws<FormatException>(() => Period.Parse(new Name("P10")));
        Assert.Throws<FormatException>(() => Period.Parse(new Name("PX")));
    }

    [Fact]
    public void TryParse_Name_Overload_Succeeds_And_Fails()
    {
        Assert.True(Period.TryParse(new Name("P0D"), out var zero));
        Assert.True(zero.HasValue);
        Assert.Equal(Period.Zero, zero.Value);

        Assert.True(Period.TryParse(new Name("ZERO"), out var z2)); // backward compatibility
        Assert.True(z2.HasValue);
        Assert.Equal(Period.Zero, z2.Value);

        Assert.False(Period.TryParse(new Name("P10"), out var _));
        Assert.False(Period.TryParse(new Name("P1W2D"), out var _)); // invalid mix of W with D
    }

    [Fact]
    public void Parse_String_Overloads()
    {
        Assert.Equal(new Period(1, 2, 0, 0), Period.Parse("P1Y2M"));
        Assert.Equal(new Period(1, 2, 0, 0), Period.Parse(" p1y2m ", provider: null)); // whitespace + case-insensitive

        // ISpanParsable Parse(string, IFormatProvider?)
        Assert.Equal(new Period(0, 0, 3, 0), Period.Parse("P3W", provider: CultureInfo.InvariantCulture));

        // ISpanParsable Parse(ReadOnlySpan<char>, IFormatProvider?)
        Assert.Equal(new Period(5, 0, 0, 6), Period.Parse("P5Y6D".AsSpan(), provider: null));
    }

    [Fact]
    public void TryParse_Provider_Overloads()
    {
        // string? overload
        Assert.True(Period.TryParse("P10D", provider: CultureInfo.InvariantCulture, out var p1));
        Assert.Equal(new Period(0, 0, 0, 10), p1);

        Assert.False(Period.TryParse((string?)null, provider: null, out var _));
        Assert.False(Period.TryParse("P", provider: null, out var _));

        // ReadOnlySpan<char> overload
        Assert.True(Period.TryParse("P2Y3M".AsSpan(), provider: null, out var p2));
        Assert.Equal(new Period(2, 3, 0, 0), p2);
    }

    [Fact]
    public void Parser_Validation_MixedWeeksAndYMD_Duplicates_Order_TrailingNumber()
    {
        // Mixed weeks with Y/M/D invalid
        Assert.False(Period.TryParse("P1W2D", out Period? _));
        Assert.False(Period.TryParse("P1Y2W", out Period? _));
        Assert.False(Period.TryParse("P1M1W", out Period? _));

        // Duplicates invalid
        Assert.False(Period.TryParse("P1Y2Y", out Period? _));
        Assert.False(Period.TryParse("P1M2M", out Period? _));
        Assert.False(Period.TryParse("P1W2W", out Period? _));
        Assert.False(Period.TryParse("P1D2D", out Period? _));

        // Trailing number invalid; "P" alone invalid
        Assert.False(Period.TryParse("P10", out Period? _));
        Assert.False(Period.TryParse("P", out Period? _));

        // Reordered units are accepted
        Assert.True(Period.TryParse("P10D1Y", out Period? r1));
        Assert.Equal(new Period(1, 0, 0, 10), r1);

        // Whitespace trimming and case-insensitive 'p'
        Assert.True(Period.TryParse("  p7d  ", out Period? r2));
        Assert.Equal(new Period(0, 0, 0, 7), r2);

        // Negative or invalid characters rejected
        Assert.False(Period.TryParse("P-1Y", out Period? _));
        Assert.False(Period.TryParse("P1X", out Period? _));
    }
}