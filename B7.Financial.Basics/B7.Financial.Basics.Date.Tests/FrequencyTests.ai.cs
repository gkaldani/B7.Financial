using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Schedule;

namespace B7.Financial.Basics.Date.Tests;

public partial class FrequencyTests
{
    [Fact]
    public void Term_Frequency_Properties()
    {
        var f = Frequency.Term;
        Assert.True(f.IsTerm);
        Assert.Equal("Term", f.ToString());
        Assert.Equal(Period.MaxValue, f.Period);
        Assert.Equal(0, f.EventsPerYear);
        Assert.Equal(0m, f.EventsPerYearEstimate);
        Assert.False(f.HasExactEventsPerYear);
        Assert.False(f.IsRegular);
    }

    [Fact]
    public void Default_Frequency_Period_Access_Throws()
    {
        Frequency f = default;
        Assert.Throws<InvalidOperationException>(() => _ = f.Period);
    }

    [Fact]
    public void Constructor_Zero_Period_Throws()
    {
        Assert.Throws<ArgumentException>(() => _ = new Frequency(Period.Zero));
    }

    [Fact]
    public void Constructor_Term_Uses_MaxValue()
    {
        var f = new Frequency(Period.MaxValue);
        Assert.True(f.IsTerm);
        Assert.Equal(Frequency.Term, f);
        Assert.Equal(Period.MaxValue, f.Period);
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
        Assert.True(f.IsMonthly);
        Assert.False(f.IsQuarterly);
        Assert.False(f.IsAnnual);
    }

    [Fact]
    public void MonthBased_NonExact_Divisor()
    {
        var f = new Frequency(Period.OfMonths(5));
        Assert.True(f.IsMonthBased);
        Assert.Equal(-1, f.EventsPerYear);
        Assert.Equal(12m / 5m, f.EventsPerYearEstimate);
        Assert.False(f.HasExactEventsPerYear);
    }

    [Fact]
    public void Quarter_And_Annual_Detection()
    {
        var quarterly = new Frequency(Period.OfMonths(3));
        Assert.True(quarterly.IsQuarterly);
        Assert.Equal(4, quarterly.EventsPerYear);

        var annual = new Frequency(Period.OfMonths(12));
        Assert.True(annual.IsAnnual);
        Assert.Equal(1, annual.EventsPerYear);
    }

    [Fact]
    public void WeekBased_Exact_And_NonExact()
    {
        var weekly = new Frequency(Period.OfWeeks(1));
        Assert.True(weekly.IsWeekBased);
        Assert.Equal(52, weekly.EventsPerYear);
        Assert.Equal(52m, weekly.EventsPerYearEstimate);

        var fiveWeek = new Frequency(Period.OfWeeks(5));
        Assert.True(fiveWeek.IsWeekBased);
        Assert.Equal(-1, fiveWeek.EventsPerYear);
        Assert.Equal(52m / 5m, fiveWeek.EventsPerYearEstimate);
    }

    [Fact]
    public void DayBased_Exact_And_NonExact()
    {
        var sevenDay = new Frequency(Period.OfDays(7)); // 364 / 7 = 52
        Assert.True(sevenDay.IsDayBased);
        Assert.False(sevenDay.IsWeekBased);
        Assert.Equal(52, sevenDay.EventsPerYear);
        Assert.Equal(52m, sevenDay.EventsPerYearEstimate);

        var tenDay = new Frequency(Period.OfDays(10));
        Assert.True(tenDay.IsDayBased);
        Assert.Equal(-1, tenDay.EventsPerYear);
        Assert.Equal(364m / 10m, tenDay.EventsPerYearEstimate);
    }

    [Fact]
    public void Mixed_Months_And_Days_NoEventsPerYear()
    {
        var mixed = new Frequency(new Period(years: 0, months: 1, days: 5));
        Assert.Equal(-1, mixed.EventsPerYear);
        Assert.Equal(-1m, mixed.EventsPerYearEstimate);
        Assert.False(mixed.HasExactEventsPerYear);
    }

    [Fact]
    public void IsRegular_Alias()
    {
        var exact = new Frequency(Period.OfMonths(6)); // 2 per year
        var inexact = new Frequency(Period.OfMonths(5));
        Assert.True(exact.IsRegular);
        Assert.False(inexact.IsRegular);
    }

    [Fact]
    public void Parse_Term_CaseInsensitive()
    {
        var a = Frequency.Parse(new Name("Term"));
        var b = Frequency.Parse(new Name("term"));
        Assert.Equal(Frequency.Term, a);
        Assert.Equal(Frequency.Term, b);
    }

    [Fact]
    public void Parse_Period()
    {
        var f = Frequency.Parse(new Name("P3M"));
        Assert.Equal(Period.OfMonths(3), f.Period);
        Assert.Equal(4, f.EventsPerYear);
    }

    [Fact]
    public void Parse_Invalid_Throws()
    {
        Assert.Throws<ArgumentException>(() => _ = Frequency.Parse(new Name("")));
        Assert.Throws<ArgumentException>(() => _ = Frequency.Parse(new Name("NOT_A_PERIOD")));
    }

    [Fact]
    public void TryParse_Success_And_Failure()
    {
        Assert.True(Frequency.TryParse(new Name("P1M"), out var f1));
        Assert.Equal(12, f1.Value.EventsPerYear);

        Assert.True(Frequency.TryParse(new Name("Term"), out var ft));
        Assert.True(ft.Value.IsTerm);

        Assert.False(Frequency.TryParse(new Name("BadValue"), out var bad));
        Assert.Null(bad);
    }

    [Fact]
    public void TryFromEventsPerYear_MonthBased()
    {
        Assert.True(Frequency.TryFromEventsPerYear(4, out var quarterly));
        Assert.Equal(Period.OfMonths(3), quarterly.Value.Period);
        Assert.Equal(4, quarterly.Value.EventsPerYear);
    }

    [Fact]
    public void TryFromEventsPerYear_WeekBased()
    {
        // 52 / 13 = 4 weeks
        Assert.True(Frequency.TryFromEventsPerYear(13, out var f));
        Assert.Equal(Period.OfWeeks(4), f.Value.Period);
        Assert.Equal(13, f.Value.EventsPerYear);
        Assert.True(f.Value.IsWeekBased);
    }

    [Fact]
    public void TryFromEventsPerYear_Zero()
    {
        Assert.True(Frequency.TryFromEventsPerYear(0, out var f));
        Assert.True(f.Value.IsTerm);
    }

    [Fact]
    public void TryFromEventsPerYear_Negative()
    {
        Assert.False(Frequency.TryFromEventsPerYear(-3, out var f));
        // Implementation sets Term when <=0
        Assert.Equal(Frequency.Term, f);
    }

    [Theory]
    [InlineData(11)]
    [InlineData(25)]
    [InlineData(17)]
    public void TryFromEventsPerYear_Invalid(int value)
    {
        var ok = Frequency.TryFromEventsPerYear(value, out var f);
        Assert.False(ok);
        Assert.Null(f);
    }

    [Fact]
    public void FromEventsPerYear_Success_And_Failure()
    {
        var monthly = Frequency.FromEventsPerYear(12);
        Assert.Equal(Period.OfMonths(1), monthly.Period);

        Assert.Throws<ArgumentException>(() => _ = Frequency.FromEventsPerYear(11));
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

    [Fact]
    public void ToNormalized_Reduces_Months()
    {
        var f = new Frequency(new Period(years: 0, months: 26, days: 0));
        var norm = f.ToNormalized();
        Assert.NotEqual(f.Period, norm.Period);
        Assert.True(norm.Period.IsNormalized);
    }

    [Fact]
    public void Addition_Operator_With_Term_Uses_Right()
    {
        var result = Frequency.Term + Period.OfMonths(3);
        Assert.Equal(Period.OfMonths(3), result.Period);
    }

    [Fact]
    public void Addition_Operator_Sums_Periods()
    {
        var a = new Frequency(Period.OfMonths(2));
        var result = a + Period.OfMonths(4);
        Assert.Equal(Period.OfMonths(6), result.Period);
    }

    [Fact]
    public void AddTo_Term_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => Frequency.Term.AddTo(new DateOnly(2024, 1, 1)));
    }

    [Fact]
    public void SubtractFrom_Term_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => Frequency.Term.SubtractFrom(new DateOnly(2024, 1, 1)));
    }

    [Fact]
    public void ToAddDateAdjuster_Works()
    {
        var f = new Frequency(Period.OfWeeks(2));
        var adj = f.ToAddDateAdjuster();
        var d = new DateOnly(2024, 1, 1);
        Assert.Equal(f.AddTo(d), adj(d));
    }

    [Fact]
    public void ToSubtractDateAdjuster_Works()
    {
        var f = new Frequency(Period.OfWeeks(3));
        var adj = f.ToSubtractDateAdjuster();
        var d = new DateOnly(2024, 2, 1);
        Assert.Equal(f.SubtractFrom(d), adj(d));
    }

    [Fact]
    public void DateAdjuster_Term_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => Frequency.Term.ToAddDateAdjuster());
        Assert.Throws<InvalidOperationException>(() => Frequency.Term.ToSubtractDateAdjuster());
    }

    [Fact]
    public void ToReciprocal_MonthBased()
    {
        var monthly = new Frequency(Period.OfMonths(1));
        var reciprocal = monthly.ToReciprocal();
        Assert.Equal(Period.OfMonths(12), reciprocal.Period);
        Assert.Equal(1, reciprocal.EventsPerYear);
    }

    [Fact]
    public void ToReciprocal_Quarterly()
    {
        var quarterly = new Frequency(Period.OfMonths(3)); // 4 events/yr
        var reciprocal = quarterly.ToReciprocal();         // 4-month period (3 events/yr)
        Assert.Equal(Period.OfMonths(4), reciprocal.Period);
        Assert.Equal(3, reciprocal.EventsPerYear);
    }

    [Fact]
    public void ToReciprocal_WeekBased()
    {
        var weekly = new Frequency(Period.OfWeeks(1));
        var reciprocal = weekly.ToReciprocal();
        Assert.Equal(Period.OfWeeks(52), reciprocal.Period);
        Assert.Equal(1, reciprocal.EventsPerYear);
    }

    [Fact]
    public void ToReciprocal_DayBased()
    {
        var sevenDay = new Frequency(Period.OfDays(7)); // 52 events/yr
        var reciprocal = sevenDay.ToReciprocal();       // 52-day => 7 events/yr
        Assert.Equal(Period.OfDays(52), reciprocal.Period);
        Assert.Equal(7, reciprocal.EventsPerYear);
    }

    [Fact]
    public void ToReciprocal_Inexact_Throws()
    {
        var inexact = new Frequency(Period.OfMonths(5));
        Assert.Throws<InvalidOperationException>(() => inexact.ToReciprocal());
    }

    [Fact]
    public void ToReciprocal_Term_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => Frequency.Term.ToReciprocal());
    }

    [Fact]
    public void TryGetReciprocal_Success_And_Failure()
    {
        var monthly = new Frequency(Period.OfMonths(1));
        Assert.True(monthly.TryGetReciprocal(out var rec));
        Assert.Equal(Period.OfMonths(12), rec.Period);

        var inexact = new Frequency(Period.OfMonths(5));
        Assert.False(inexact.TryGetReciprocal(out _));
    }

    [Fact]
    public void IsCompatibleWith_Tests()
    {
        var monthly = new Frequency(Period.OfMonths(1));
        var quarterly = new Frequency(Period.OfMonths(3));
        var weekly = new Frequency(Period.OfWeeks(1));
        var fiveDay = new Frequency(Period.OfDays(5));

        Assert.True(monthly.IsCompatibleWith(quarterly));
        Assert.True(quarterly.IsCompatibleWith(monthly));
        Assert.True(Frequency.Term.IsCompatibleWith(monthly));
        Assert.True(monthly.IsCompatibleWith(Frequency.Term));

        Assert.True(weekly.IsCompatibleWith(new Frequency(Period.OfWeeks(4))));
        Assert.True(fiveDay.IsCompatibleWith(new Frequency(Period.OfDays(7))));

        Assert.False(monthly.IsCompatibleWith(weekly));
        Assert.False(weekly.IsCompatibleWith(fiveDay)); // week vs day
        Assert.False(fiveDay.IsCompatibleWith(monthly));
    }
}