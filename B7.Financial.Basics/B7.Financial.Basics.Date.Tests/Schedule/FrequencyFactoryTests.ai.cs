using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Schedule;
using B7.Financial.Basics.Date.Schedule;

namespace B7.Financial.Basics.Date.Tests.Schedule;

public partial class FrequencyFactoryTests
{
    private readonly FrequencyFactory _factoryFactory = new();

    [Fact]
    public void Of_KnownName_ReturnsPredefined()
    {
        var f = _factoryFactory.Of(new Name("P1M"));
        Assert.Equal(FrequencyFactory.P1M, f);
        Assert.Equal(Period.OfMonths(1), f.Period);
    }

    [Fact]
    public void Of_KnownName_CaseInsensitive()
    {
        var fLower = _factoryFactory.Of(new Name("p1m"));
        Assert.Equal(FrequencyFactory.P1M, fLower);
    }

    [Fact]
    public void Of_UnknownPeriodName_ParsesNewFrequency()
    {
        var f = _factoryFactory.Of(new Name("P5M"));
        Assert.Equal(Period.OfMonths(5), f.Period);
        // Not one of the predefined (those months: 1,2,3,4,6,12)
        Assert.NotEqual(FrequencyFactory.P4M, f);
        Assert.NotEqual(FrequencyFactory.P6M, f);
    }

    [Fact]
    public void Of_InvalidName_Throws()
    {
        Assert.Throws<ArgumentException>(() => _factoryFactory.Of(new Name("BAD_VALUE")));
        Assert.Throws<ArgumentException>(() => _factoryFactory.Of(new Name("")));
    }

    [Fact]
    public void FrequencyNames_ReturnsAllPredefined()
    {
        var names = _factoryFactory.FrequencyNames().ToArray();
        Assert.Equal(14, names.Length);

        var expected = new[]
        {
            FrequencyFactory.Term.Name,
            FrequencyFactory.P1D.Name,
            FrequencyFactory.P1W.Name, FrequencyFactory.P2W.Name, FrequencyFactory.P4W.Name,
            FrequencyFactory.P13W.Name, FrequencyFactory.P26W.Name, FrequencyFactory.P52W.Name,
            FrequencyFactory.P1M.Name, FrequencyFactory.P2M.Name, FrequencyFactory.P3M.Name,
            FrequencyFactory.P4M.Name, FrequencyFactory.P6M.Name, FrequencyFactory.P12M.Name
        };

        Assert.True(expected.All(e => names.Contains(e)));
    }

    [Fact]
    public void OfDays_Positive_ReturnsFrequency()
    {
        var f10 = FrequencyFactory.OfDays(10);
        Assert.Equal(Period.OfDays(10), f10.Period);

        var f1 = FrequencyFactory.OfDays(1);
        Assert.Equal(FrequencyFactory.P1D, f1);
    }

    [Fact]
    public void OfDays_Zero_Throws()
    {
        Assert.Throws<ArgumentException>(() => FrequencyFactory.OfDays(0));
    }

    [Fact]
    public void OfWeeks_PredefinedValues_ReturnCached()
    {
        Assert.Equal(FrequencyFactory.P1W, FrequencyFactory.OfWeeks(1));
        Assert.Equal(FrequencyFactory.P2W, FrequencyFactory.OfWeeks(2));
        Assert.Equal(FrequencyFactory.P4W, FrequencyFactory.OfWeeks(4));
        Assert.Equal(FrequencyFactory.P13W, FrequencyFactory.OfWeeks(13));
        Assert.Equal(FrequencyFactory.P26W, FrequencyFactory.OfWeeks(26));
        Assert.Equal(FrequencyFactory.P52W, FrequencyFactory.OfWeeks(52));
    }

    [Fact]
    public void OfWeeks_NonPredefined_ReturnsNew()
    {
        var f5 = FrequencyFactory.OfWeeks(5);
        Assert.Equal(Period.OfWeeks(5), f5.Period);
        // Ensure not equal to a predefined one
        Assert.NotEqual(FrequencyFactory.P4W, f5);
        Assert.NotEqual(FrequencyFactory.P13W, f5);
    }

    [Fact]
    public void OfMonths_PredefinedValues_ReturnCached()
    {
        Assert.Equal(FrequencyFactory.P1M, FrequencyFactory.OfMonths(1));
        Assert.Equal(FrequencyFactory.P2M, FrequencyFactory.OfMonths(2));
        Assert.Equal(FrequencyFactory.P3M, FrequencyFactory.OfMonths(3));
        Assert.Equal(FrequencyFactory.P4M, FrequencyFactory.OfMonths(4));
        Assert.Equal(FrequencyFactory.P6M, FrequencyFactory.OfMonths(6));
        Assert.Equal(FrequencyFactory.P12M, FrequencyFactory.OfMonths(12));
    }

    [Fact]
    public void OfMonths_NonPredefined_ReturnsNew()
    {
        var f5 = FrequencyFactory.OfMonths(5);
        Assert.Equal(Period.OfMonths(5), f5.Period);
        Assert.NotEqual(FrequencyFactory.P4M, f5);
    }

    [Fact]
    public void OfYears_ReturnsYearPeriodFrequency()
    {
        var f2 = FrequencyFactory.OfYears(2);
        Assert.Equal(Period.OfYears(2), f2.Period);

        var f1 = FrequencyFactory.OfYears(1);
        Assert.Equal(Period.OfYears(1), f1.Period);
        // Distinguish from 12M predefined (if equality is by structural period)
        Assert.NotEqual(FrequencyFactory.P12M.Period, f1.Period);
    }

    [Fact]
    public void Static_Predefined_Term_Equals_FrequencyTerm()
    {
        Assert.Equal(Frequency.Term, FrequencyFactory.Term);
        Assert.True(FrequencyFactory.Term.IsTerm);
    }
}