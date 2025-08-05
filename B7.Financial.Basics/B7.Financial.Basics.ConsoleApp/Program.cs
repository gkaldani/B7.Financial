using B7.Financial.Basics.Date.PeriodIso8601;

namespace B7.Financial.Basics.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var period = Period.Parse("P1YM35d");

        Console.WriteLine(period);
    }
}