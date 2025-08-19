namespace B7.Financial.Abstractions.Date.HolidayCalendars;

/// <summary>
/// Represents a factory for creating Holiday Calendars.
/// </summary>
public interface IHolidayCalendarFactory : INamedFactory<IHolidayCalendar>
{
    /// <summary>
    /// Retrieves the names of all available holiday calendars.
    /// </summary>
    /// <returns> A collection of holiday calendar names. </returns>
    IEnumerable<Name> HolidayCalendarNames();


    /// <summary>
    /// Combines this holiday calendar with another.
    /// <para>
    /// The resulting calendar will declare a day as a business day if it is a <br/>
    /// business day in both source calendars.
    /// </para>
    /// </summary>
    /// <param name="calendar1">Calendar 1</param>
    /// <param name="calendar2">Calendar 2</param>
    /// <returns>The combined calendar</returns>
    IHolidayCalendar Combine(IHolidayCalendar calendar1, IHolidayCalendar calendar2);

    /// <summary>
    /// Links two holiday calendars together.
    /// <para>
    /// The resulting calendar will declare a day as a business day if it is a <br/>
    /// business day in either source calendar.
    /// </para>
    /// </summary>
    /// <param name="calendar1">Calendar 1</param>
    /// <param name="calendar2">Calendar 2</param>
    /// <returns>The linked calendar</returns>
    IHolidayCalendar Link(IHolidayCalendar calendar1, IHolidayCalendar calendar2);
}