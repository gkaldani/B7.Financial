using B7.Financial.Abstractions;

namespace B7.Financial.Basics.Date.Tests;

public class NameTests
{
    [Fact]
    public void Constructor_ValidName_ShouldInitialize()
    {
        // Arrange
        var validName = "Act/Act ISDA";

        // Act
        var name = new Name(validName);

        // Assert
        Assert.Equal(validName, name.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidName_ShouldThrowArgumentException(string invalidName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Name(invalidName));
        Assert.Equal("Name cannot be null or whitespace. (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void Constructor_NameExceedsMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var longName = new string('A', Name.MaxLength + 1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Name(longName));
        Assert.Contains($"Name cannot be longer than {Name.MaxLength} characters.", exception.Message);
    }

    [Fact]
    public void Create_ValidName_ShouldReturnNameInstance()
    {
        // Arrange
        var validName = "Act/Act ISDA";

        // Act
        var name = Name.Create(validName);

        // Assert
        Assert.Equal(validName, name.Value);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var name = new Name("Act/Act ISDA");

        // Act
        var result = name.ToString();

        // Assert
        Assert.Equal("Act/Act ISDA", result);
    }

    [Fact]
    public void Equals_SameValue_ShouldReturnTrue()
    {
        // Arrange
        var name1 = new Name("Act/Act ISDA");
        var name2 = new Name("act/act ISDA");

        // Act
        var result = name1.Equals(name2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var name1 = new Name("Act/Act ISDA");
        var name2 = new Name("Act/Act ICMA");

        // Act
        var result = name1.Equals(name2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_SameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        var name1 = new Name("Act/Act ISDA");
        var name2 = new Name("act/act isda");

        // Act
        var hash1 = name1.GetHashCode();
        var hash2 = name2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var name = new Name("Act/Act ISDA");

        // Act
        string result = name;

        // Assert
        Assert.Equal("Act/Act ISDA", result);
    }

    [Fact]
    public void ImplicitConversion_ToReadOnlySpan_ShouldReturnSpan()
    {
        // Arrange
        var name = new Name("Act/Act ISDA");

        // Act
        ReadOnlySpan<char> span = name;

        // Assert
        Assert.Equal("Act/Act ISDA".AsSpan().ToString(), span.ToString());
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldCreateName()
    {
        // Arrange
        string value = "Act/Act ISDA";

        // Act
        Name name = value;

        // Assert
        Assert.Equal(value, name.Value);
    }
}