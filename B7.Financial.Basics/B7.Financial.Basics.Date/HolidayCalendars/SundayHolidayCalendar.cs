using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Basics.Date.HolidayCalendars;

/// <summary>
/// An identifier for a calendar declaring all days as business days <br/>
/// except Sunday weekends, with code 'Sun'.
/// </summary>
public sealed class SundayHolidayCalendar : HolidayCalendar
{
    /// <summary>
    /// The name of the day Holiday Calendar.
    /// </summary>
    public static readonly Name CalendarName = "Sun";

    /// <inheritdoc />
    public override Name Name => CalendarName;

    /// <inheritdoc />
    public override bool IsHoliday(DateOnly date) => date.DayOfWeek == DayOfWeek.Sunday;
}