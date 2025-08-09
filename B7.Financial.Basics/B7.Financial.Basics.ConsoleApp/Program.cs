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

        IDayCountFactory dayCountFactory = new StandardDayCountsFactory();

        IDayCount oneOneDayCount = dayCountFactory.Of(DayCountActualActualIsda.Name);

        Console.WriteLine(oneOneDayCount.GetType());
        
        Console.WriteLine(DayCountActualActualIsda.Name);
        Console.WriteLine(oneOneDayCount.GetName());

        //var dayCount = dayCountFactory.Of("Actual/365");

        Console.WriteLine(period);

        var termDeposit = new TermDeposit(oneOneDayCount);

        Console.WriteLine(termDeposit);

    }
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
        return $"TermDeposit:\r\n\tDayCount: {DayCount.GetName()}";
    }
}