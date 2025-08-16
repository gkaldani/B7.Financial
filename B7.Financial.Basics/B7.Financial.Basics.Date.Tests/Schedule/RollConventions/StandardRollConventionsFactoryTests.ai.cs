using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Schedule;
using B7.Financial.Basics.Date.Schedule.RollConventions;

namespace B7.Financial.Basics.Date.Tests.Schedule.RollConventions;

public class StandardRollConventionsFactoryTests
{
    private readonly StandardRollConventionsFactory _factory = new();

    [Fact]
    public void Of_KnownNames_ReturnsSameStaticInstances()
    {
        var expectedInstances = new IRollConvention[]
        {
            StandardRollConventionsFactory.None,

            StandardRollConventionsFactory.Dom1, StandardRollConventionsFactory.Dom2, StandardRollConventionsFactory.Dom3, StandardRollConventionsFactory.Dom4, StandardRollConventionsFactory.Dom5,
            StandardRollConventionsFactory.Dom6, StandardRollConventionsFactory.Dom7, StandardRollConventionsFactory.Dom8, StandardRollConventionsFactory.Dom9, StandardRollConventionsFactory.Dom10,
            StandardRollConventionsFactory.Dom11, StandardRollConventionsFactory.Dom12, StandardRollConventionsFactory.Dom13, StandardRollConventionsFactory.Dom14, StandardRollConventionsFactory.Dom15,
            StandardRollConventionsFactory.Dom16, StandardRollConventionsFactory.Dom17, StandardRollConventionsFactory.Dom18, StandardRollConventionsFactory.Dom19, StandardRollConventionsFactory.Dom20,
            StandardRollConventionsFactory.Dom21, StandardRollConventionsFactory.Dom22, StandardRollConventionsFactory.Dom23, StandardRollConventionsFactory.Dom24, StandardRollConventionsFactory.Dom25,
            StandardRollConventionsFactory.Dom26, StandardRollConventionsFactory.Dom27, StandardRollConventionsFactory.Dom28, StandardRollConventionsFactory.Dom29, StandardRollConventionsFactory.Dom30,
            StandardRollConventionsFactory.Eom,

            StandardRollConventionsFactory.Monday, StandardRollConventionsFactory.Tuesday, StandardRollConventionsFactory.Wednesday,
            StandardRollConventionsFactory.Thursday, StandardRollConventionsFactory.Friday, StandardRollConventionsFactory.Saturday, StandardRollConventionsFactory.Sunday,

            StandardRollConventionsFactory.Imm, StandardRollConventionsFactory.Immnzd, StandardRollConventionsFactory.Sfe
        };

        foreach (var expected in expectedInstances)
        {
            var result = _factory.Of(expected.Name);
            Assert.Same(expected, result);
        }
    }

    [Fact]
    public void Of_NameCaseInsensitive_ReturnsSameInstance()
    {
        var original = StandardRollConventionsFactory.Dom15;
        var lower = new Name(original.Name.Value.ToLowerInvariant());

        var result = _factory.Of(lower);

        Assert.Same(original, result);
    }

    [Fact]
    public void Of_UnknownName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _factory.Of(new Name("UNKNOWN_ROLL_CONVENTION")));
    }

    [Fact]
    public void RollConventionNames_ReturnsAllPredefined()
    {
        var names = _factory.RollConventionNames().ToArray();

        // Expected count: 1 (None) + 30 (Dom1..Dom30) + 1 (Eom) + 7 (Mon..Sun) + 3 (Imm, Immnzd, Sfe) = 42
        Assert.Equal(42, names.Length);
        Assert.Equal(42, names.Distinct().Count());

        // Spot check a few
        Assert.Contains(StandardRollConventionsFactory.None.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Dom1.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Dom30.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Eom.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Monday.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Sunday.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Imm.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Immnzd.Name, names);
        Assert.Contains(StandardRollConventionsFactory.Sfe.Name, names);
    }

    [Fact]
    public void OfDayOfMonth_1_To_30_ReturnsExpectedStaticInstances()
    {
        var expectedByIndex = new IRollConvention[]
        {
            StandardRollConventionsFactory.Dom1, StandardRollConventionsFactory.Dom2, StandardRollConventionsFactory.Dom3, StandardRollConventionsFactory.Dom4, StandardRollConventionsFactory.Dom5,
            StandardRollConventionsFactory.Dom6, StandardRollConventionsFactory.Dom7, StandardRollConventionsFactory.Dom8, StandardRollConventionsFactory.Dom9, StandardRollConventionsFactory.Dom10,
            StandardRollConventionsFactory.Dom11, StandardRollConventionsFactory.Dom12, StandardRollConventionsFactory.Dom13, StandardRollConventionsFactory.Dom14, StandardRollConventionsFactory.Dom15,
            StandardRollConventionsFactory.Dom16, StandardRollConventionsFactory.Dom17, StandardRollConventionsFactory.Dom18, StandardRollConventionsFactory.Dom19, StandardRollConventionsFactory.Dom20,
            StandardRollConventionsFactory.Dom21, StandardRollConventionsFactory.Dom22, StandardRollConventionsFactory.Dom23, StandardRollConventionsFactory.Dom24, StandardRollConventionsFactory.Dom25,
            StandardRollConventionsFactory.Dom26, StandardRollConventionsFactory.Dom27, StandardRollConventionsFactory.Dom28, StandardRollConventionsFactory.Dom29, StandardRollConventionsFactory.Dom30
        };

        for (byte day = 1; day <= 30; day++)
        {
            var rc = StandardRollConventionsFactory.OfDayOfMonth(day);
            Assert.Same(expectedByIndex[day - 1], rc);
        }
    }

    [Fact]
    public void OfDayOfMonth_31_ReturnsEndOfMonth()
    {
        var rc = StandardRollConventionsFactory.OfDayOfMonth(31);
        Assert.Same(StandardRollConventionsFactory.Eom, rc);
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)32)]
    public void OfDayOfMonth_OutOfRange_Throws(byte invalid)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => StandardRollConventionsFactory.OfDayOfMonth(invalid));
    }

    [Fact]
    public void OfDayOfWeek_ReturnsCorrectTypeAndName_ForEachDay()
    {
        var map = new (DayOfWeek Dow, Type Type, Name ExpectedName)[]
        {
            (DayOfWeek.Monday, typeof(RollConventionDowMonday), StandardRollConventionsFactory.Monday.Name),
            (DayOfWeek.Tuesday, typeof(RollConventionDowTuesday), StandardRollConventionsFactory.Tuesday.Name),
            (DayOfWeek.Wednesday, typeof(RollConventionDowWednesday), StandardRollConventionsFactory.Wednesday.Name),
            (DayOfWeek.Thursday, typeof(RollConventionDowThursday), StandardRollConventionsFactory.Thursday.Name),
            (DayOfWeek.Friday, typeof(RollConventionDowFriday), StandardRollConventionsFactory.Friday.Name),
            (DayOfWeek.Saturday, typeof(RollConventionDowSaturday), StandardRollConventionsFactory.Saturday.Name),
            (DayOfWeek.Sunday, typeof(RollConventionDowSunday), StandardRollConventionsFactory.Sunday.Name)
        };

        foreach (var (dow, type, expectedName) in map)
        {
            var rc = StandardRollConventionsFactory.OfDayOfWeek(dow);
            Assert.IsType(type, rc);
            Assert.Equal(expectedName, rc.Name);
        }
    }

    [Fact]
    public void OfDayOfWeek_NewInstanceEachCall()
    {
        var rc1 = StandardRollConventionsFactory.OfDayOfWeek(DayOfWeek.Monday);
        var rc2 = StandardRollConventionsFactory.OfDayOfWeek(DayOfWeek.Monday);

        Assert.NotSame(rc1, rc2); // Implementation currently returns a new instance
        Assert.Equal(rc1.Name, rc2.Name);
    }

    [Fact]
    public void OfDayOfWeek_InvalidEnumValue_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => StandardRollConventionsFactory.OfDayOfWeek((DayOfWeek)7));
    }
}