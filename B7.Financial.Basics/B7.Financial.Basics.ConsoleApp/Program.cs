using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;
using B7.Financial.Basics.Date.DayCountConventions;
using B7.Financial.Basics.Date.PeriodIso8601;

namespace B7.Financial.Basics.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var period = Period.Parse("P1Y35d");

        var adjuster = period.ToAddDateAdjuster();

        var d = adjuster(DateOnly.FromDateTime(DateTime.Now));

        Console.WriteLine($"Adjusted date: {d}");

        IDayCountFactory dayCountFactory = new StandardDayCountFactory();

        foreach (var dayCountName in dayCountFactory.DayCountNames())
        {
            Console.WriteLine(dayCountName);
        }

        IDayCount dayCount = dayCountFactory.Of("Act/Act ISDA");

        Console.WriteLine(dayCount.GetType());
        
        //Console.WriteLine(DayCountActualActualIsda.Name);
        Console.WriteLine(dayCount.Name);

        //var dayCount = dayCountFactory.Of("Actual/365");

        Console.WriteLine(period);

        var termDeposit = new TermDeposit(dayCount);

        Console.WriteLine(termDeposit);

        var depositCreated = new DepositCreated
        {
            DayCount = dayCount
        };

        //Serialize the event to JSON with NamedJsonConverter
        var options = new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new DayCountJsonConverter(dayCountFactory)
            }
        };
        // Serialize the DepositCreated event
        var json = System.Text.Json.JsonSerializer.Serialize(depositCreated, options);

        Console.WriteLine("Serialized DepositCreated event:");
        Console.WriteLine(json);

        //Deserialize the event from JSON
        var deserializedEvent = System.Text.Json.JsonSerializer.Deserialize<DepositCreated>(json, options);

        Console.WriteLine("Deserialized DepositCreated event:");
        Console.WriteLine($"DayCount: {deserializedEvent?.DayCount.Name}");

        var period2 = PeriodBuilder.Create()
            .WithYears(1)
            .WithMonths(13)
            .WithDays(35)
            .Normalize()
            .Build();

        Console.WriteLine("PeriodBuilder example:");
        Console.WriteLine(period2);

    }
}

public interface IEvent;

public sealed class DepositCreated : IEvent
{
    public required IDayCount DayCount { get; init; }
}

public class TermDeposit
{
    public IDayCount DayCount { get; }
    public TermDeposit(IDayCount dayCount)
    {
        DayCount = dayCount;
    }

    public override string ToString()
    {
        return $"TermDeposit:\r\n\tDayCount: {DayCount.Name}";
    }
}

// Json converter for IDayCount as string

public class DayCountJsonConverter(IDayCountFactory factory) : System.Text.Json.Serialization.JsonConverter<IDayCount>
{
    public override IDayCount Read(ref System.Text.Json.Utf8JsonReader reader, Type typeToConvert, System.Text.Json.JsonSerializerOptions options) =>
        factory.Of(reader.GetString() ?? throw new InvalidOperationException());

    public override void Write(System.Text.Json.Utf8JsonWriter writer, IDayCount value, System.Text.Json.JsonSerializerOptions options) =>
        writer.WriteStringValue(value.Name.AsSpan());
}
