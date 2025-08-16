
using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Schedule;
using B7.Financial.Basics.Date.Schedule.RollConventions;

namespace B7.Financial.Basics.Date.Tests.Schedule.RollConventions;

public class RollConventionEomTests
{
    private static Frequency OneMonth => new(Period.OfMonths(1));
    private static Frequency TwoMonths => new(Period.OfMonths(2));

    [Fact]
    public void Name_ShouldBeStaticValue()
    {
        var rc = new RollConventionEom();
        Assert.Equal(RollConventionEom.RollConventionName, rc.Name);
        Assert.Equal("EOM", rc.Name.Value);
    }

    [Theory]
    [InlineData(2023, 1, 10, 2023, 1, 31)]
    [InlineData(2023, 4, 15, 2023, 4, 30)]
    [InlineData(2023, 2, 1, 2023, 2, 28)]  // Non-leap Feb
    [InlineData(2024, 2, 10, 2024, 2, 29)] // Leap Feb
    [InlineData(2024, 12, 5, 2024, 12, 31)]
    public void Adjust_ShouldReturnLastDayOfMonth(int y, int m, int d, int ey, int em, int ed)
    {
        var rc = new RollConventionEom();
        var input = new DateOnly(y, m, d);

        var adjusted = rc.Adjust(input);

        Assert.Equal(new DateOnly(ey, em, ed), adjusted);
    }

    [Theory]
    [InlineData(2023, 1, 31)]
    [InlineData(2023, 4, 30)]
    [InlineData(2023, 2, 28)]
    [InlineData(2024, 2, 29)]
    public void Adjust_Idempotent_WhenAlreadyEom(int y, int m, int d)
    {
        var rc = new RollConventionEom();
        var eom = new DateOnly(y, m, d);

        var adjusted = rc.Adjust(eom);

        Assert.Equal(eom, adjusted);
    }

    [Fact]
    public void DayOfMonth_ShouldReturn31_SentinelForEom()
    {
        var rc = new RollConventionEom();
        Assert.Equal((byte)31, rc.DayOfMonth);
    }

    [Fact]
    public void Matches_ShouldReturnTrue_ForEndOfMonth()
    {
        var rc = new RollConventionEom();
        var date = new DateOnly(2024, 2, 29);

        Assert.True(rc.Matches(date));
    }

    [Fact]
    public void Matches_ShouldReturnFalse_ForNonEndOfMonth()
    {
        var rc = new RollConventionEom();
        var date = new DateOnly(2024, 2, 27);

        Assert.False(rc.Matches(date));
    }

    [Fact]
    public void Next_ShouldReturnNextMonthEom_FromMiddleOfMonth()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 1, 15);

        var next = rc.Next(start, OneMonth);

        Assert.Equal(new DateOnly(2024, 2, 29), next);
        Assert.True(next > start);
    }

    [Fact]
    public void Next_ShouldReturnEom_WhenCrossingTo30DayMonth()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 3, 20); // March

        var next = rc.Next(start, OneMonth);

        Assert.Equal(new DateOnly(2024, 4, 30), next);
    }

    [Fact]
    public void Next_ShouldHandleAlreadyEom_Source()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 1, 31);

        var next = rc.Next(start, OneMonth);

        Assert.Equal(new DateOnly(2024, 2, 29), next);
    }

    [Fact]
    public void Next_WithMultiMonthFrequency_ShouldReturnTargetMonthEom()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 3, 31); // EOM

        var next = rc.Next(start, TwoMonths); // +2 months -> May

        Assert.Equal(new DateOnly(2024, 5, 31), next);
    }

    [Fact]
    public void Previous_ShouldReturnPriorMonthEom_FromMiddleOfMonth()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 3, 20);

        var previous = rc.Previous(start, OneMonth);

        Assert.Equal(new DateOnly(2024, 2, 29), previous);
        Assert.True(previous < start);
    }

    [Fact]
    public void Previous_ShouldHandleAlreadyEom_Source()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 3, 31);

        var previous = rc.Previous(start, OneMonth);

        Assert.Equal(new DateOnly(2024, 2, 29), previous);
    }

    [Fact]
    public void Previous_WithMultiMonthFrequency_ShouldReturnTargetMonthEom()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 7, 31);

        var previous = rc.Previous(start, TwoMonths); // -2 months -> May

        Assert.Equal(new DateOnly(2024, 5, 31), previous);
    }
}