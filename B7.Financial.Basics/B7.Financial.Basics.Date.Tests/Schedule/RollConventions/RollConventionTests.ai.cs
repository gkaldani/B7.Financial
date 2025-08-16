using B7.Financial.Abstractions.Date;
using B7.Financial.Abstractions.Schedule;
using B7.Financial.Basics.Date.Schedule.RollConventions;

namespace B7.Financial.Basics.Date.Tests.Schedule.RollConventions;

public partial class RollConventionTests
{
    private static Frequency Monthly => new(Period.OfMonths(1));

    [Fact]
    public void Matches_ShouldReturnTrue_WhenDateAlreadyAdjusted_None()
    {
        var rc = new RollConventionNone();
        var date = new DateOnly(2024, 1, 10);

        var result = rc.Matches(date);

        Assert.True(result);
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenDateAdjustsToDifferent_Eom()
    {
        var rc = new RollConventionEom();
        var date = new DateOnly(2024, 1, 15); // Not end of month

        var result = rc.Matches(date);

        Assert.False(result);
    }

    [Fact]
    public void Matches_ShouldReturnTrue_WhenDateIsEndOfMonth_Eom()
    {
        var rc = new RollConventionEom();
        var date = new DateOnly(2024, 2, 29); // Leap year Feb EOM

        var result = rc.Matches(date);

        Assert.True(result);
    }

    [Fact]
    public void Next_ShouldReturnAdjustedDate_WhenAfterOriginal()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 1, 15);

        var next = rc.Next(start, Monthly);

        Assert.Equal(new DateOnly(2024, 2, 29), next);
        Assert.True(next > start);
    }

    [Fact]
    public void Next_ShouldFallbackAddOneMonth_WhenAdjustmentNotAfter()
    {
        var rc = new RollConventionNone();
        var start = new DateOnly(2024, 1, 10);

        var next = rc.Next(start, Monthly);

        Assert.Equal(new DateOnly(2024, 2, 10), next);
    }

    [Fact]
    public void Previous_ShouldReturnAdjustedDate_WhenBeforeOriginal()
    {
        var rc = new RollConventionEom();
        var start = new DateOnly(2024, 3, 20);

        var previous = rc.Previous(start, Monthly);

        Assert.Equal(new DateOnly(2024, 2, 29), previous);
        Assert.True(previous < start);
    }

    [Fact]
    public void Previous_ShouldFallbackSubtractOneMonth_WhenAdjustmentNotBefore()
    {
        var rc = new RollConventionNone();
        var start = new DateOnly(2024, 3, 10);

        var previous = rc.Previous(start, Monthly);

        Assert.Equal(new DateOnly(2024, 2, 10), previous);
    }

    [Fact]
    public void DayOfMonth_Default_ShouldReturnZero_ForNone()
    {
        var rc = new RollConventionNone();

        Assert.Equal((byte)0, rc.DayOfMonth);
    }

    [Fact]
    public void DayOfMonth_Eom_ShouldReturn31()
    {
        var rc = new RollConventionEom();

        Assert.Equal((byte)31, rc.DayOfMonth);
    }

    [Fact]
    public void DayOfMonth_Dom15_ShouldReturn15()
    {
        var rc = new RollConventionDom15();

        Assert.Equal((byte)15, rc.DayOfMonth);
    }
}