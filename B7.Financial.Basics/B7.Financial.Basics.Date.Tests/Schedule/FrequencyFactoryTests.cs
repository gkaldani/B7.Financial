using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Schedule;
using B7.Financial.Basics.Date.Schedule;

namespace B7.Financial.Basics.Date.Tests.Schedule;

public partial class FrequencyFactoryTests
{
    public class TestFrequencyPeriodNameClass : TheoryData<Frequency, Period, Name, string>
    {
        public TestFrequencyPeriodNameClass()
        {
            Add(Frequency.Term, Period.MaxValue, Frequency.Term.Name, "Term");

            Add(FrequencyFactory.OfDays(1), Period.OfDays(1), new Name("P1D"), "P1D");
            Add(FrequencyFactory.OfDays(2), Period.OfDays(2), new Name("P2D"), "P2D");
            Add(FrequencyFactory.OfDays(30), Period.OfDays(30), new Name("P30D"), "P30D");

            Add(FrequencyFactory.OfWeeks(1), Period.OfWeeks(1), new Name("P1W"), "P1W");
            Add(FrequencyFactory.OfWeeks(2), Period.OfWeeks(2), new Name("P2W"), "P2W");
            Add(FrequencyFactory.OfWeeks(4), Period.OfWeeks(4), new Name("P4W"), "P4W");
            Add(FrequencyFactory.OfWeeks(13), Period.OfWeeks(13), new Name("P13W"), "P13W");
            Add(FrequencyFactory.OfWeeks(26), Period.OfWeeks(26), new Name("P26W"), "P26W");
            Add(FrequencyFactory.OfWeeks(52), Period.OfWeeks(52), new Name("P52W"), "P52W");

            Add(FrequencyFactory.OfMonths(1), Period.OfMonths(1), new Name("P1M"), "P1M");
            Add(FrequencyFactory.OfMonths(2), Period.OfMonths(2), new Name("P2M"), "P2M");
            Add(FrequencyFactory.OfMonths(3), Period.OfMonths(3), new Name("P3M"), "P3M");
            Add(FrequencyFactory.OfMonths(4), Period.OfMonths(4), new Name("P4M"), "P4M");
            Add(FrequencyFactory.OfMonths(6), Period.OfMonths(6), new Name("P6M"), "P6M");
            Add(FrequencyFactory.OfMonths(12), Period.OfMonths(12), new Name("P12M"), "P12M");

            Add(FrequencyFactory.OfYears(1), Period.OfYears(1), new Name("P1Y"), "P1Y");
            Add(FrequencyFactory.OfYears(2), Period.OfYears(2), new Name("P2Y"), "P2Y");
            Add(FrequencyFactory.OfYears(5), Period.OfYears(5), new Name("P5Y"), "P5Y");
            Add(FrequencyFactory.OfYears(10), Period.OfYears(10), new Name("P10Y"), "P10Y");
        }
    }

    [Theory]
    [ClassData(typeof(TestFrequencyPeriodNameClass))]
    public void FrequencyFactory_ExpectedFrequencies(Frequency frequency, Period period, Name name, string toString)
    {
        // Assert
        Assert.Equal(frequency.Period, period);
        Assert.Equal(frequency.Name, name);
        Assert.Equal(frequency.ToString(), toString);
    }

    [Theory]
    [ClassData(typeof(TestFrequencyPeriodNameClass))]
    public void FrequencyFactory_OfPeriod_ReturnsExpectedFrequency(Frequency frequency, Period period, Name name, string toString)
    {
        // Act
        var result =  _factoryFactory.Of(name);
        // Assert
        Assert.Equal(frequency.Period, result.Period);
        Assert.Equal(frequency.Name, result.Name);
        Assert.Equal(frequency.ToString(), result.ToString());
    }

    public class TestFrequencyOfMonthsClass : TheoryData<int, Frequency, Name, string>
    {
        public TestFrequencyOfMonthsClass()
        {
            Add(1, FrequencyFactory.OfMonths(1), new Name("P1M"), "P1M");
            Add(2, FrequencyFactory.OfMonths(2), new Name("P2M"), "P2M");
            Add(3, FrequencyFactory.OfMonths(3), new Name("P3M"), "P3M");
            Add(4, FrequencyFactory.OfMonths(4), new Name("P4M"), "P4M");
            Add(5, FrequencyFactory.OfMonths(5), new Name("P5M"), "P5M");
            Add(6, FrequencyFactory.OfMonths(6), new Name("P6M"), "P6M");
            Add(12, FrequencyFactory.OfMonths(12), new Name("P12M"), "P12M");
        }
    }

    [Theory]
    [ClassData(typeof(TestFrequencyOfMonthsClass))]
    public void FrequencyFactory_OfMonths_ReturnsExpectedFrequency(int months, Frequency frequency, Name name, string toString)
    {
        // Act
        var result = _factoryFactory.Of(name);
        
        // Assert
        Assert.Equal(frequency.Period, result.Period);
        Assert.Equal(frequency.Name, result.Name);
        Assert.Equal(frequency.ToString(), result.ToString());
    }

    public class TestFrequencyOfDaysClass : TheoryData<int, Frequency, Name, string>
    {
        public TestFrequencyOfDaysClass()
        {
            Add(1, FrequencyFactory.OfDays(1), new Name("P1D"), "P1D");
            Add(2, FrequencyFactory.OfDays(2), new Name("P2D"), "P2D");
            Add(3, FrequencyFactory.OfDays(3), new Name("P3D"), "P3D");
            Add(5, FrequencyFactory.OfDays(5), new Name("P5D"), "P5D");
            Add(10, FrequencyFactory.OfDays(10), new Name("P10D"), "P10D");
            Add(30, FrequencyFactory.OfDays(30), new Name("P30D"), "P30D");
        }
    }

    [Theory]
    [ClassData(typeof(TestFrequencyOfDaysClass))]
    public void FrequencyFactory_OfDays_ReturnsExpectedFrequency(int days, Frequency frequency, Name name, string toString)
    {
        // Act
        var result = _factoryFactory.Of(name);
        
        // Assert
        Assert.Equal(frequency.Period, result.Period);
        Assert.Equal(frequency.Name, result.Name);
        Assert.Equal(frequency.ToString(), result.ToString());
    }

    public class TestNormalizedFrequencyPeriodClass : TheoryData<Frequency, Period>
    {
        public TestNormalizedFrequencyPeriodClass()
        {
            Add(Frequency.Term, Period.MaxValue);
            Add(FrequencyFactory.OfDays(1), Period.OfDays(1));
            Add(FrequencyFactory.OfDays(7), Period.OfDays(7));
            Add(FrequencyFactory.OfDays(10), Period.OfDays(10));
            Add(FrequencyFactory.OfMonths(1), Period.OfMonths(1));
            Add(FrequencyFactory.OfMonths(2), Period.OfMonths(2));
            Add(FrequencyFactory.OfMonths(30).ToNormalized(), new Period(years: 2, months: 6, days: 0));
            Add(FrequencyFactory.OfMonths(12).ToNormalized(), Period.OfYears(1));
            Add(FrequencyFactory.OfYears(1), Period.OfYears(1));
            Add(FrequencyFactory.OfYears(2), Period.OfYears(2));
        }
    }

    [Theory]
    [ClassData(typeof(TestNormalizedFrequencyPeriodClass))]
    public void FrequencyFactory_NormalizedFrequency_ReturnsExpectedPeriod(Frequency frequency, Period expectedPeriod)
    {
        // Act
        var normalized = frequency.ToNormalized();
        
        // Assert
        Assert.Equal(expectedPeriod, normalized.Period);
    }

    public class TestEventsPerYearClass : TheoryData<Frequency, int>
    {
        public TestEventsPerYearClass()
        {
            Add(FrequencyFactory.P1D, 364);
            Add(FrequencyFactory.P1W, 52);
            Add(FrequencyFactory.P2W, 26);
            Add(FrequencyFactory.P4W, 13);
            Add(FrequencyFactory.P13W, 4);
            Add(FrequencyFactory.P26W, 2);
            Add(FrequencyFactory.P52W, 1);
            Add(FrequencyFactory.P1M, 12);
            Add(FrequencyFactory.P2M, 6);
            Add(FrequencyFactory.P3M, 4);
            Add(FrequencyFactory.P4M, 3);
            Add(FrequencyFactory.P6M, 2);
            Add(FrequencyFactory.P12M, 1);
            Add(FrequencyFactory.P1Y, 1);
            Add(FrequencyFactory.Term, 0); // Term has no fixed events per year
        }
    }

    [Theory]
    [ClassData(typeof(TestEventsPerYearClass))]
    public void FrequencyFactory_EventsPerYear_ReturnsExpectedCount(Frequency frequency, int expectedCount)
    {
        // Assert
        Assert.Equal(expectedCount, frequency.EventsPerYear);
    }

    public class TestExactDiversityClass : TheoryData<Frequency, Frequency, int>
    {
        public TestExactDiversityClass()
        {
            Add(FrequencyFactory.P1D, FrequencyFactory.P1D, 1);

            Add(FrequencyFactory.P1W, FrequencyFactory.P1W, 1);
            Add(FrequencyFactory.P2W, FrequencyFactory.P1W, 2);
            Add(FrequencyFactory.OfWeeks(3), FrequencyFactory.P1W, 3);
            Add(FrequencyFactory.P4W, FrequencyFactory.P4W, 1);
            Add(FrequencyFactory.P4W, FrequencyFactory.P1W, 4);
            Add(FrequencyFactory.P4W, FrequencyFactory.P2W, 2);
            Add(FrequencyFactory.P13W, FrequencyFactory.P13W, 1);
            Add(FrequencyFactory.P13W, FrequencyFactory.P1W, 13);
            Add(FrequencyFactory.P26W, FrequencyFactory.P1W, 26);
            Add(FrequencyFactory.P26W, FrequencyFactory.P13W, 2);
            Add(FrequencyFactory.P26W, FrequencyFactory.P26W, 1);
            Add(FrequencyFactory.P52W, FrequencyFactory.P52W, 1);
            Add(FrequencyFactory.P52W, FrequencyFactory.P26W, 2);
            Add(FrequencyFactory.P52W, FrequencyFactory.P13W, 4);
            Add(FrequencyFactory.P52W, FrequencyFactory.P2W, 26);
            Add(FrequencyFactory.P52W, FrequencyFactory.P1W, 52);

            Add(FrequencyFactory.P1M, FrequencyFactory.P1M, 1);
            Add(FrequencyFactory.P2M, FrequencyFactory.P2M, 1);
            Add(FrequencyFactory.P2M, FrequencyFactory.P1M, 2);
            Add(FrequencyFactory.P3M, FrequencyFactory.P3M, 1);
            Add(FrequencyFactory.P3M, FrequencyFactory.P1M, 3);
            Add(FrequencyFactory.P4M, FrequencyFactory.P4M, 1);
            Add(FrequencyFactory.P4M, FrequencyFactory.P2M, 2);
            Add(FrequencyFactory.P4M, FrequencyFactory.P1M, 4);
            Add(FrequencyFactory.P6M, FrequencyFactory.P6M, 1);
            Add(FrequencyFactory.P6M, FrequencyFactory.P3M, 2);
            Add(FrequencyFactory.P6M, FrequencyFactory.P2M, 3);
            Add(FrequencyFactory.P6M, FrequencyFactory.P1M, 6);
            Add(FrequencyFactory.P12M, FrequencyFactory.P12M, 1);
            Add(FrequencyFactory.P12M, FrequencyFactory.P1M, 12);
            Add(FrequencyFactory.P12M, FrequencyFactory.P2M, 6);
        }
    }

    [Theory]
    [ClassData(typeof(TestExactDiversityClass))]
    public void FrequencyFactory_ExactDiversity_ReturnsExpectedCount(Frequency frequency, Frequency otherFrequency, int expectedCount)
    {
        // Act
        var diversity = frequency.ExactDivide(otherFrequency);
        
        // Assert
        Assert.Equal(expectedCount, diversity);
    }
}