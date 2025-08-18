using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.HolidayCalendars;


/// <summary>
/// An identifier for a calendar declaring all days as business days <br/>
/// except Saturday/Sunday weekends, with code 'Sat/Sun'.
/// </summary>
public sealed class SatSunHolidayCalendar : HolidayCalendar
{
    /// <summary>
    /// The name of the day Holiday Calendar.
    /// </summary>
    public static readonly Name CalendarName = "Sat/Sun";

    /// <inheritdoc />
    public override Name Name => CalendarName;

    /// <inheritdoc />
    public override bool IsHoliday(DateOnly date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}