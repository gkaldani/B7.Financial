using System.Text.Json;
using System.Text.Json.Serialization;

namespace B7.Financial.Abstractions.Date;

/// <summary>
/// JSON converter for <see cref="IDayCount"/>.
/// </summary>
/// <param name="factory"></param>
public sealed class DayCountJsonConverter(IDayCountFactory factory) : JsonConverter<IDayCount>
{
    /// <summary>
    /// Reads a <see cref="IDayCount"/> from JSON.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override IDayCount Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        factory.Of(reader.GetString() ?? throw new InvalidOperationException());

    /// <summary>
    /// Writes a <see cref="IDayCount"/> to JSON as a string representation of its name.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, IDayCount value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Name.AsSpan());
}