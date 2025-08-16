using System.Text;
using B7.Financial.Abstractions;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace B7.Financial.Basics.Date.Tests;

public class NamePropertyBasedTests
{
    #region Custom Generators

    /// <summary>
    /// Generator for valid name strings (non-null, non-whitespace, within max length)
    /// </summary>
    public static Arbitrary<string> ValidNameStrings() =>
        Arb.From(
            from length in Gen.Choose(1, Name.MaxLength)
            from chars in Gen.Choose(33, 126).ArrayOf(length) // Printable ASCII chars
            let str = new string(chars.Select(c => (char)c).ToArray())
            where !string.IsNullOrWhiteSpace(str) && str.Trim().Length <= Name.MaxLength
            select str);

    /// <summary>
    /// Generator for invalid name strings (null, empty, whitespace, or too long)
    /// </summary>
    public static Arbitrary<string> InvalidNameStrings() =>
        Arb.From(Gen.OneOf(
            Gen.Constant<string>(null!),
            Gen.Constant(string.Empty),
            Gen.Constant("   "), // whitespace only
            Gen.Constant("\t\n\r"), // various whitespace chars
            // Too long strings
            from length in Gen.Choose(Name.MaxLength + 1, Name.MaxLength + 100)
            from chars in Gen.Choose(65, 90).ArrayOf(length) // A-Z
            select new string(chars.Select(c => (char)c).ToArray())
        ));

    /// <summary>
    /// Generator for strings that need trimming
    /// </summary>
    public static Arbitrary<string> StringsNeedingTrim() =>
        Arb.From(
            from coreLength in Gen.Choose(1, Name.MaxLength - 10) // Leave room for whitespace
            from chars in Gen.Choose(65, 90).ArrayOf(coreLength)
            let core = new string(chars.Select(c => (char)c).ToArray())
            from leftSpaces in Gen.Choose(1, 5)
            from rightSpaces in Gen.Choose(1, 5)
            select new string(' ', leftSpaces) + core + new string(' ', rightSpaces));

    #endregion

    #region Construction Tests

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)], MaxTest = 100)]
    public Property ValidName_Construction_Succeeds(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(validName);
                return name.Value == validName.Trim().Normalize(NormalizationForm.FormC);
            }).Collect($"validName is {validName}");
    }

    [Property]
    public void InvalidName_Construction_ThrowsException(NonNull<string> input)
    {
        var value = input.Item;
        
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > Name.MaxLength)
        {
            Assert.ThrowsAny<ArgumentException>(() => new Name(value));
        }
    }

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Create_EquivalentToConstructor(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var nameFromConstructor = new Name(validName);
                var nameFromCreate = Name.Create(validName);
                return nameFromConstructor.Equals(nameFromCreate);
            });
    }

    #endregion

    #region Value Property Tests

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Value_IsTrimmedAndNormalized(string input)
    {
        return (!string.IsNullOrWhiteSpace(input) && input.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(input);
                var expected = input.Trim().Normalize(NormalizationForm.FormC);
                return name.Value == expected;
            });
    }

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Value_NeverExceedsMaxLength(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(validName);
                return name.Value.Length <= Name.MaxLength;
            });
    }

    #endregion

    #region Equality Tests

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Equality_IsReflexive(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(validName);
                return name.Equals(name);
            });
    }

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Equality_IsSymmetric(string validName1, string validName2)
    {
        return (!string.IsNullOrWhiteSpace(validName1) && validName1.Trim().Length <= Name.MaxLength &&
                !string.IsNullOrWhiteSpace(validName2) && validName2.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name1 = new Name(validName1);
                var name2 = new Name(validName2);
                return name1.Equals(name2) == name2.Equals(name1);
            });
    }

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Equality_IsTransitive(string validName1, string validName2, string validName3)
    {
        return (!string.IsNullOrWhiteSpace(validName1) && validName1.Trim().Length <= Name.MaxLength &&
                !string.IsNullOrWhiteSpace(validName2) && validName2.Trim().Length <= Name.MaxLength &&
                !string.IsNullOrWhiteSpace(validName3) && validName3.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name1 = new Name(validName1);
                var name2 = new Name(validName2);
                var name3 = new Name(validName3);
                
                if (name1.Equals(name2) && name2.Equals(name3))
                {
                    return name1.Equals(name3);
                }
                return true; // Property vacuously true if premise doesn't hold
            });
    }

    [Property]
    public Property Name_Equality_IsCaseInsensitive(NonEmptyString input)
    {
        var baseString = input.Item;
        if (baseString.Trim().Length > Name.MaxLength) 
            return true.ToProperty(); // Skip if too long
        
        return (!string.IsNullOrWhiteSpace(baseString))
            .ToProperty()
            .And(() =>
            {
                var lowerName = new Name(baseString.ToLowerInvariant());
                var upperName = new Name(baseString.ToUpperInvariant());
                return lowerName.Equals(upperName);
            });
    }

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_GetHashCode_ConsistentWithEquals(string validName1, string validName2)
    {
        return (!string.IsNullOrWhiteSpace(validName1) && validName1.Trim().Length <= Name.MaxLength &&
                !string.IsNullOrWhiteSpace(validName2) && validName2.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name1 = new Name(validName1);
                var name2 = new Name(validName2);
                
                if (name1.Equals(name2))
                {
                    return name1.GetHashCode() == name2.GetHashCode();
                }
                return true; // Hash codes may or may not be equal if objects aren't equal
            });
    }

    #endregion

    #region Conversion Tests

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_ImplicitToString_ReturnsValue(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(validName);
                string converted = name; // Implicit conversion
                return converted == name.Value;
            });
    }

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_ImplicitFromString_CreatesValidName(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                Name name = validName; // Implicit conversion
                return name.Value == validName.Trim().Normalize(NormalizationForm.FormC);
            });
    }

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Conversion_RoundTrip(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                Name name = validName; // string -> Name
                string backToString = name; // Name -> string
                Name backToName = backToString; // string -> Name
                
                return name.Equals(backToName);
            });
    }

    #endregion

    #region ToString Tests

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_ToString_ReturnsValue(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(validName);
                return name.ToString() == name.Value;
            });
    }

    #endregion

    #region AsSpan Tests

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_AsSpan_ReturnsValueSpan(string validName)
    {
        return (!string.IsNullOrWhiteSpace(validName) && 
                validName.Trim().Length <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(validName);
                var span = name.AsSpan();
                var valueSpan = name.Value.AsSpan();
                return span.SequenceEqual(valueSpan);
            });
    }

    #endregion

    #region Trimming and Normalization Tests

    [Property(Arbitrary = [typeof(NamePropertyBasedTests)])]
    public Property Name_Constructor_TrimsWhitespace(string input)
    {
        // Only test strings that after trimming would be valid
        var trimmed = input.Trim();
        return (!string.IsNullOrWhiteSpace(input) && 
                trimmed.Length is > 0 and <= Name.MaxLength)
            .ToProperty()
            .And(() =>
            {
                var name = new Name(input);
                return name.Value == trimmed.Normalize(NormalizationForm.FormC);
            });
    }

    [Property]
    public Property Name_Constructor_NormalizesUnicode(NonEmptyString input)
    {
        var testString = input.Item;
        if (testString.Trim().Length > Name.MaxLength || string.IsNullOrWhiteSpace(testString))
            return true.ToProperty(); // Skip invalid cases
        
        // Add some Unicode characters that can be normalized
        var unicodeString = testString + "\u0065\u0301"; // 'e' + combining acute accent
        if (unicodeString.Trim().Length > Name.MaxLength)
            return true.ToProperty(); // Skip if too long after Unicode addition
        
        return true.ToProperty().And(() =>
        {
            var name = new Name(unicodeString);
            var expected = unicodeString.Trim().Normalize(NormalizationForm.FormC);
            return name.Value == expected;
        });
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void Name_DefaultValue_ThrowsOnValueAccess()
    {
        var defaultName = default(Name);
        Assert.Throws<InvalidOperationException>(() => defaultName.Value);
    }

    [Property]
    public Property Name_MaxLength_Boundary(PositiveInt length)
    {
        var actualLength = Math.Min(length.Item, Name.MaxLength);
        var testString = new string('A', actualLength);
        
        return true.ToProperty().And(() =>
        {
            var name = new Name(testString);
            return name.Value.Length == actualLength;
        });
    }

    [Fact]
    public void Name_MaxLengthPlusOne_ThrowsException()
    {
        var tooLongString = new string('A', Name.MaxLength + 1);
        Assert.Throws<ArgumentOutOfRangeException>(() => new Name(tooLongString));
    }

    #endregion
}