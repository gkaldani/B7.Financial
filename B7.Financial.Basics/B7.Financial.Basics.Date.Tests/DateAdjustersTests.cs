namespace B7.Financial.Basics.Date.Tests;

public class DateAdjustersTests
{
    [Theory]
    [InlineData(2023, 1, 1, 2024, 2, 29)] // Non-leap year, next leap day
    [InlineData(2024, 2, 28, 2024, 2, 29)] // Leap year, before Feb 29
    [InlineData(2024, 2, 29, 2024, 2, 29)] // Already a leap day
    [InlineData(2100, 3, 1, 2104, 2, 29)] // 2100 is not a leap year
    public void NextLeapDay_ReturnsExpectedDate(int startYear, int startMonth, int startDay, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var startDate = new DateOnly(startYear, startMonth, startDay);
        var expectedDate = new DateOnly(expectedYear, expectedMonth, expectedDay);

        // Act
        var result = startDate.NextLeapDay();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData(2023, 1, 1, 2024, 2, 29)] // Non-leap year, next leap day
    [InlineData(2024, 2, 28, 2024, 2, 29)] // Leap year, before Feb 29
    [InlineData(2024, 2, 29, 2024, 2, 29)] // Already a leap day
    [InlineData(2100, 3, 1, 2104, 2, 29)] // 2100 is not a leap year
    public void NextLeapDayAdjuster_ReturnsExpectedDate(int startYear, int startMonth, int startDay, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var startDate = new DateOnly(startYear, startMonth, startDay);
        var expectedDate = new DateOnly(expectedYear, expectedMonth, expectedDay);

        // Act
        var result = DateAdjusters.NextLeapDayAdjuster(startDate);

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData(2023, 1, 1, 2024, 2, 29)] // Non-leap year, next leap day
    [InlineData(2024, 2, 28, 2024, 2, 29)] // Leap year, before Feb 29
    [InlineData(2024, 2, 29, 2024, 2, 29)] // Already a leap day
    [InlineData(2100, 3, 1, 2104, 2, 29)] // 2100 is not a leap year
    public void NextOrSameLeapDay_ReturnsExpectedDate(int startYear, int startMonth, int startDay, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var startDate = new DateOnly(startYear, startMonth, startDay);
        var expectedDate = new DateOnly(expectedYear, expectedMonth, expectedDay);

        // Act
        var result = startDate.NextOrSameLeapDay();

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData(2023, 1, 1, 2024, 2, 29)] // Non-leap year, next leap day
    [InlineData(2024, 2, 28, 2024, 2, 29)] // Leap year, before Feb 29
    [InlineData(2024, 2, 29, 2024, 2, 29)] // Already a leap day
    [InlineData(2100, 3, 1, 2104, 2, 29)] // 2100 is not a leap year
    public void NextOrSameLeapDayAdjuster_ReturnsExpectedDate(int startYear, int startMonth, int startDay, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Arrange
        var startDate = new DateOnly(startYear, startMonth, startDay);
        var expectedDate = new DateOnly(expectedYear, expectedMonth, expectedDay);

        // Act
        var result = DateAdjusters.NextOrSameLeapDayAdjuster(startDate);

        // Assert
        Assert.Equal(expectedDate, result);
    }
}