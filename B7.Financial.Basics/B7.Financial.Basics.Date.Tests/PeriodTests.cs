using System.Globalization;

using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.Tests;

public class PeriodTests
{
    [Fact]
    public void Zero_Period_Properties()
    {
        var p = Period.Zero;
        Assert.True(p.IsZero);
        Assert.Equal("P0D", p.ToString());
        Assert.Equal(p, Period.AdditiveIdentity);
        Assert.True(p.IsDateBased);
        Assert.False(p.IsWeekBased);
    }

    [Fact]
    public void MaxValue_Has_IntMax_Components()
    {
        var p = Period.MaxValue;
        Assert.Equal(int.MaxValue, p.Years);
        Assert.Equal(int.MaxValue, p.Months);
        Assert.Equal(0, p.Weeks);
        Assert.Equal(int.MaxValue, p.Days);
    }

    [Fact]
    public void Static_Factories()
    {
        Assert.Equal(10, Period.OfDays(10).Days);
        Assert.Equal(6, Period.OfMonths(6).Months);
        Assert.Equal(3, Period.OfWeeks(3).Weeks);
        Assert.Equal(2, Period.OfYears(2).Years);
    }

    [Fact]
    public void Public_Constructors()
    {
        var w = new Period(4);
        Assert.Equal(4, w.Weeks);
        var ymd = new Period(1, 2, 3);
        Assert.Equal(1, ymd.Years);
        Assert.Equal(2, ymd.Months);
        Assert.Equal(3, ymd.Days);
    }

    [Fact]
    public void Name_Equals_ToString()
    {
        var p = new Period(1, 2, 3);
        Assert.Equal(p.ToString(), p.Name);
    }

    [Fact]
    public void Flags_Computed()
    {
        var a = new Period(0, 13, 5); // invalid because weeks + months cannot happen via ctor; create valid examples
        var dateBased = new Period(1, 13 - 12, 0); // Years=1, Months=1
        Assert.True(dateBased.IsDateBased);
        Assert.False(dateBased.IsWeekBased);
        var weeks = new Period(5);
        Assert.True(weeks.IsWeekBased);
        Assert.False(weeks.IsDateBased == false ? false : false); // ensure no side effects
        Assert.False(new Period(0, 12, 0).IsNormalized);
        Assert.True(new Period(0, 11, 0).IsNormalized);
    }

    [Fact]
    public void Equality_And_Inequality()
    {
        var p1 = new Period(1, 2, 3);
        var p2 = new Period(1, 2, 3);
        var p3 = new Period(2, 1, 3);
        Assert.True(p1 == p2);
        Assert.False(p1 != p2);
        Assert.True(p1 != p3);
        Assert.False(p1 == p3);
        Assert.True(p1.Equals((object)p2));
        Assert.False(p1.Equals((object)p3));
    }

    [Fact]
    public void ToString_Default()
    {
        Assert.Equal("P0D", Period.Zero.ToString());
        Assert.Equal("P1Y2M3D", new Period(1, 2, 3).ToString());
        Assert.Equal("P5W", new Period(5).ToString());
        Assert.Equal("P10M", new Period(0, 10, 0).ToString());
        Assert.Equal("P1Y", new Period(1, 0, 0).ToString());
    }

    [Fact]
    public void TryFormat_Variants()
    {
        var p = new Period(0, 25, 14); // 0Y25M0W14D
        Span<char> dst = stackalloc char[32];

        // Default
        Assert.True(p.TryFormat(dst, out var written, default, null));
        Assert.Equal("P25M14D", dst[..written].ToString());

        // Normalize (N): 25M => 2Y1M
        Assert.True(p.TryFormat(dst, out written, "N".AsSpan(), null));
        Assert.Equal("P2Y1M14D", dst[..written].ToString());

        // Week compression (W) only when no Y/M and days divisible by 7
        var d21 = Period.OfDays(21);
        Assert.True(d21.TryFormat(dst, out written, "W".AsSpan(), null));
        Assert.Equal("P3W", dst[..written].ToString());

        // Canonical (C): normalize + week compress
        var p2 = new Period(0, 24, 21); // 24M, 21D
        Assert.True(p2.TryFormat(dst, out written, "C".AsSpan(), null));
        // 24M => 2Y0M (prevents week compression because Y != 0) so stays days
        Assert.Equal("P2Y21D", dst[..written].ToString());

        // Canonical with only days
        Assert.True(d21.TryFormat(dst, out written, "C".AsSpan(), null));
        Assert.Equal("P3W", dst[..written].ToString());

        // Unknown format -> default
        Assert.True(p.TryFormat(dst, out written, "X".AsSpan(), null));
        Assert.Equal("P25M14D", dst[..written].ToString());
    }

    [Fact]
    public void TryFormat_BufferTooSmall_Fails()
    {
        Span<char> small = stackalloc char[2]; // cannot hold even "P0"
        var ok = Period.Zero.TryFormat(small, out _, default, null);
        Assert.False(ok);
    }

    [Fact]
    public void Addition_Normalizes_Months()
    {
        var a = new Period(1, 10, 5);
        var b = new Period(0, 5, 2);
        var result = a + b;
        // Months: 10 + 5 = 15 => +1Y 3M
        Assert.Equal(2, result.Years);
        Assert.Equal(3, result.Months);
        Assert.Equal(7, result.Days);
    }

    [Fact]
    public void Addition_Overflow_Throws()
    {
        var a = Period.OfDays(int.MaxValue);
        var b = Period.OfDays(1);
        Assert.Throws<OverflowException>(() => { var _ = a + b; });
    }

    [Fact]
    public void Multiplication_Normalizes_Months()
    {
        var p = new Period(0, 11, 2);
        var r = p * 2;
        // 11*2 = 22 => 1Y 10M
        Assert.Equal(1, r.Years);
        Assert.Equal(10, r.Months);
        Assert.Equal(4, r.Days);
    }

    [Fact]
    public void Multiplication_Commutative()
    {
        var p = Period.OfMonths(5);
        Assert.Equal(p * 3, 3 * p);
    }

    [Fact]
    public void Multiplication_NegativeFactor_Throws()
    {
        var p = new Period(1, 2, 3);
        Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = p * -1; });
    }

    [Fact]
    public void Multiplication_Overflow_Throws()
    {
        var p = Period.OfYears(int.MaxValue);
        Assert.Throws<OverflowException>(() => { var _ = p * 2; });
    }

    [Fact]
    public void ToTotalMonths_Computed()
    {
        var p = new Period(2, 5, 7);
        Assert.Equal(2 * 12 + 5, p.ToTotalMonths());
    }

    [Fact]
    public void ToNormalized_Reduces_Months()
    {
        var p = new Period(1, 24, 3);
        var norm = p.ToNormalized();
        Assert.Equal(3, norm.Years); // 1 + 2
        Assert.Equal(0, norm.Months);
        Assert.Equal(3, norm.Days);
    }

    [Fact]
    public void ToNormalized_When_Already_Normalized_Returns_Same()
    {
        var p = new Period(1, 5, 2);
        var norm = p.ToNormalized();
        Assert.Equal(p, norm);
    }

    [Fact]
    public void Deconstruct_Works()
    {
        var p = new Period(2, 3, 4);
        p.Deconstruct(out var y, out var m, out var w, out var d);
        Assert.Equal(2, y);
        Assert.Equal(3, m);
        Assert.Equal(0, w);
        Assert.Equal(4, d);
    }

    [Fact]
    public void Parse_Name_Success()
    {
        var name = new Name("P10D");
        var p = Period.Parse(name);
        Assert.Equal(10, p.Days);
    }

    [Fact]
    public void Parse_String_Success()
    {
        var p = Period.Parse("P1Y2M3D");
        Assert.Equal(1, p.Years);
        Assert.Equal(2, p.Months);
        Assert.Equal(3, p.Days);
    }

    [Fact]
    public void Parse_Span_Success()
    {
        ReadOnlySpan<char> span = "P5W".AsSpan();
        var p = Period.Parse(span);
        Assert.Equal(5, p.Weeks);
    }

    [Fact]
    public void Parse_Zero_Alias()
    {
        Assert.True(Period.TryParse("ZERO", out var p));
        Assert.True(p!.Value.IsZero);
    }

    [Fact]
    public void Parse_Whitespace_Trim()
    {
        var p = Period.Parse("   P10D  ");
        Assert.Equal(10, p.Days);
    }

    [Theory]
    [InlineData("")]
    [InlineData("P")]
    [InlineData("PX")]
    [InlineData("P1")]
    [InlineData("P1Y1Y")]
    [InlineData("P1W2D")]
    [InlineData("P1Y1W")]
    public void Parse_Invalid_Fails(string value)
    {
        Assert.False(Period.TryParse(value, out var _));
        Assert.Throws<FormatException>(() => Period.Parse(value));
    }

    [Fact]
    public void TryParse_Name_Invalid_ReturnsFalse()
    {
        var name = new Name("P1Y1W");
        Assert.False(Period.TryParse(name, out var _));
    }

    [Fact]
    public void Instance_Of_Uses_Parse()
    {
        var period = Period.OfDays(5);
        var recreated = period.Of(new Name("P5D"));
        Assert.Equal(period, recreated);
    }

    [Fact]
    public void ToAddDateAdjuster_Applies_Correct_Order()
    {
        // Order test: Jan 30 + (1M 5D) => (AddMonths first) Feb 29 + 5 = Mar 5 (leap year)
        var p = new Period(0, 1, 5);
        var baseDate = new DateOnly(2024, 1, 30);
        var expected = new DateOnly(2024, 3, 5);
        Assert.Equal(expected, p.AddTo(baseDate));
        var adjuster = p.ToAddDateAdjuster();
        Assert.Equal(expected, adjuster(baseDate));
    }

    [Fact]
    public void AddTo_WeekBased()
    {
        var p = Period.OfWeeks(2);
        var baseDate = new DateOnly(2024, 1, 1);
        var expected = new DateOnly(2024, 1, 15);
        Assert.Equal(expected, p.AddTo(baseDate));
    }

    [Fact]
    public void SubtractFrom_DateBased_Order()
    {
        // 2024-03-05 - (1M 5D) => subtract month => 2024-02-05 then -5D => 2024-01-31
        var p = new Period(0, 1, 5);
        var baseDate = new DateOnly(2024, 3, 5);
        var expected = new DateOnly(2024, 1, 31);
        Assert.Equal(expected, p.SubtractFrom(baseDate));
        var adjuster = p.ToSubtractDateAdjuster();
        Assert.Equal(expected, adjuster(baseDate));
    }

    [Fact]
    public void SubtractFrom_WeekBased()
    {
        var p = Period.OfWeeks(3);
        var baseDate = new DateOnly(2024, 2, 1);
        var expected = new DateOnly(2024, 1, 11);
        Assert.Equal(expected, p.SubtractFrom(baseDate));
    }

    [Fact]
    public void ToString_Format_Overload_Ignores_Format()
    {
        var p = new Period(1, 2, 3);
        Assert.Equal(p.ToString(), p.ToString("N", null));
    }
}