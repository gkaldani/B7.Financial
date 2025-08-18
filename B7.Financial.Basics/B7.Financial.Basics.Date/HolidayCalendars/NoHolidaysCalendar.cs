using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.HolidayCalendars;

/// <summary>
/// An identifier for a calendar declaring no holidays and no weekends, with code 'NoHolidays'.
/// <para>
/// This calendar has the effect of making every day a business day. <br/>
/// It is often used to indicate that a holiday calendar does not apply.
/// </para>
/// </summary>
public sealed class NoHolidaysCalendar : HolidayCalendar
{
    /// <summary>
    /// No Holidays Calendar.
    /// </summary>
    public static readonly IHolidayCalendar NoHolidays = new NoHolidaysCalendar();
    /// <summary>
    /// The name of the day Holiday Calendar.
    /// </summary>
    public static readonly Name CalendarName = "NoHolidays";

    /// <inheritdoc />
    public override Name Name => CalendarName;

    /// <inheritdoc />
    public override bool IsHoliday(DateOnly date) => false;
}