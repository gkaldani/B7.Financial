using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Schedule;

namespace B7.Financial.Basics.Date.Tests;

public class FrequencyTests
{
    [Fact]
    public void Term_Frequency_Properties()
    {
        var f = Frequency.Term;
        Assert.True(f.IsTerm);
        Assert.Equal("Term", f.ToString());
        Assert.Equal(Period.Zero, f.Period);
        Assert.Equal(0, f.EventsPerYear);
        Assert.Equal(0m, f.EventsPerYearEstimate);
        Assert.False(f.HasExactEventsPerYear);
        Assert.False(f.IsRegular);
    }

    [Fact]
    public void Constructor_From_Zero_Period_Equals_Term()
    {
        var constructed = new Frequency(Period.Zero);
        Assert.Equal(Frequency.Term, constructed);
        Assert.True(constructed.IsTerm);
    }

    [Fact]
    public void MonthBased_Exact_Divisor()
    {
        var f = new Frequency(Period.OfMonths(1)); // monthly
        Assert.True(f.IsMonthBased);
        Assert.Equal(12, f.EventsPerYear);
        Assert.Equal(12m, f.EventsPerYearEstimate);
        Assert.True(f.HasExactEventsPerYear);
        Assert.True(f.IsRegular);
    }

    [Fact]
    public void MonthBased_NonExact_Divisor()
    {
        var f = new Frequency(Period.OfMonths(5));
        Assert.Equal(-1, f.EventsPerYear);
        Assert.Equal(12m / 5m, f.EventsPerYearEstimate);
        Assert.False(f.HasExactEventsPerYear);
    }

    [Fact]
    public void DayBased_Exact_Divisor()
    {
        var f = new Frequency(Period.OfDays(7)); // 364 / 7 = 52
        Assert.False(f.IsMonthBased);
        Assert.False(f.IsWeekBased); // underlying Period uses days, not weeks
        Assert.Equal(52, f.EventsPerYear);
        Assert.Equal(52m, f.EventsPerYearEstimate);
    }

    [Fact]
    public void DayBased_NonExact_Divisor()
    {
        var f = new Frequency(Period.OfDays(10));
        Assert.Equal(-1, f.EventsPerYear);
        Assert.Equal(364m / 10m, f.EventsPerYearEstimate);
    }

    [Fact]
    public void WeekBased_Exact_Divisor()
    {
        var f = new Frequency(Period.OfWeeks(1));
        Assert.True(f.IsWeekBased);
        Assert.Equal(52, f.EventsPerYear);
        Assert.Equal(52m, f.EventsPerYearEstimate);
    }

    [Fact]
    public void WeekBased_NonExact_Divisor()
    {
        var f = new Frequency(Period.OfWeeks(5));
        Assert.True(f.IsWeekBased);
        Assert.Equal(-1, f.EventsPerYear);
        Assert.Equal(52m / 5m, f.EventsPerYearEstimate);
    }

    [Fact]
    public void Mixed_Months_And_Days_NoEventsPerYear()
    {
        var f = new Frequency(new Period(0, 1, 5));
        Assert.Equal(-1, f.EventsPerYear);
        Assert.Equal(-1m, f.EventsPerYearEstimate);
        Assert.False(f.HasExactEventsPerYear);
    }

    [Fact]
    public void IsAnnual_Detection()
    {
        var annual = new Frequency(Period.OfMonths(12));
        Assert.True(annual.IsAnnual);
        Assert.True(annual.IsMonthBased);
        Assert.Equal(1, annual.EventsPerYear); // 12 months => 1 event per year
    }

    [Fact]
    public void IsMonthBased_Property()
    {
        var f = new Frequency(Period.OfMonths(3));
        Assert.True(f.IsMonthBased);
        Assert.False(f.IsWeekBased);
    }

    [Fact]
    public void IsWeekBased_Property()
    {
        var f = new Frequency(Period.OfWeeks(2));
        Assert.True(f.IsWeekBased);
        Assert.False(f.IsMonthBased);
    }

    [Fact]
    public void IsRegular_Alias_For_HasExactEventsPerYear()
    {
        var exact = new Frequency(Period.OfMonths(6)); // 2 per year
        Assert.True(exact.HasExactEventsPerYear);
        Assert.True(exact.IsRegular);

        var inexact = new Frequency(Period.OfMonths(5));
        Assert.False(inexact.HasExactEventsPerYear);
        Assert.False(inexact.IsRegular);
    }

    [Fact]
    public void TryFromEventsPerYear_MonthBased_Success()
    {
        Assert.True(Frequency.TryFromEventsPerYear(4, out var f)); // quarterly
        Assert.Equal(Period.OfMonths(3), f.Value.Period);
        Assert.Equal(4, f.Value.EventsPerYear);
        Assert.True(f.Value.IsMonthBased);
    }

    [Fact]
    public void TryFromEventsPerYear_WeekBased_Success()
    {
        // 52 / 13 = 4 weeks
        Assert.True(Frequency.TryFromEventsPerYear(13, out var f));
        Assert.Equal(Period.OfWeeks(4), f.Value.Period);
        Assert.Equal(13, f.Value.EventsPerYear);
        Assert.True(f.Value.IsWeekBased);
    }

    [Fact]
    public void TryFromEventsPerYear_DayBased_Success()
    {
        // 364 / 14 = 26 events
        Assert.True(Frequency.TryFromEventsPerYear(26, out var f));
        Assert.Equal(Period.OfWeeks(2), f.Value.Period);
        Assert.Equal(26, f.Value.EventsPerYear);
        Assert.True(f.Value.IsWeekBased);
        Assert.False(f.Value.IsMonthBased);
    }

    [Theory]
    //[InlineData(0)]
    //[InlineData(-5)]
    [InlineData(11)]   // not divisor of 12, 52, or 364
    [InlineData(25)]   // not divisor
    public void TryFromEventsPerYear_Invalid_ReturnsFalse(int value)
    {
        Frequency ff = default; 
        var ok = Frequency.TryFromEventsPerYear(value, out var f);
        Assert.False(ok);
        if (value <= 0)
        {
            Assert.Equal(Frequency.Term, f); // implementation sets Term on non-positive
        }
        else
        {
            Assert.Equal(ff, f);
        }
    }

    [Fact]
    public void Deconstruct_ReturnsComponents()
    {
        var f = new Frequency(Period.OfMonths(2));
        f.Deconstruct(out var p, out var e, out var est);
        Assert.Equal(f.Period, p);
        Assert.Equal(f.EventsPerYear, e);
        Assert.Equal(f.EventsPerYearEstimate, est);
    }

    [Fact]
    public void ToString_Equals_Name()
    {
        var f = new Frequency(Period.OfMonths(2));
        Assert.Equal(f.Name.ToString(), f.ToString());
    }

    [Fact]
    public void Equality_And_Inequality()
    {
        var a1 = new Frequency(Period.OfMonths(2));
        var a2 = new Frequency(Period.OfMonths(2));
        var b = new Frequency(Period.OfMonths(3));
        Assert.True(a1 == a2);
        Assert.False(a1 != a2);
        Assert.True(a1 != b);
        Assert.False(a1 == b);
        Assert.True(a1.Equals((object)a2));
        Assert.False(a1.Equals((object)b));
    }

    [Fact]
    public void GetHashCode_Same_For_Equal()
    {
        var a = new Frequency(Period.OfMonths(6));
        var b = new Frequency(Period.OfMonths(6));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }
}