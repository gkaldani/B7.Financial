using B7.Financial.Basics.Date;
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

        var oneOneDayCount = dayCountFactory.Of(DayCountOneOne.Name);

        Console.WriteLine(DayCountOneOne.Name);
        Console.WriteLine(oneOneDayCount.GetName());

        //var dayCount = dayCountFactory.Of("Actual/365");

        Console.WriteLine(period);
    }
}