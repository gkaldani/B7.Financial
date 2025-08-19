using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date.HolidayCalendars;
using System.Collections.Concurrent;

namespace B7.Financial.Basics.Date.HolidayCalendars;

/// <inheritdoc />
public class HolidayCalendarFactory : IHolidayCalendarFactory
{
    /// <summary> No Holidays Calendar. </summary>
    public static readonly IHolidayCalendar NoHolidays = NoHolidaysCalendar.NoHolidays;

    /// <summary> Sunday </summary>
    public static readonly SundayHolidayCalendar Sun = new();

    /// <summary> Saturday/Sunday </summary>
    public static readonly SatSunHolidayCalendar SatSun = new();

    private static readonly ConcurrentDictionary<Name, IHolidayCalendar> HolidayCalendars =
        new()
        {
            [NoHolidays.Name] = NoHolidays,
            [Sun.Name] = Sun,
            [SatSun.Name] = SatSun,
        };

    private const char CombinedJoiner = CombinedHolidayCalendar.Joiner;
    private const char LinkedJoiner = LinkedHolidayCalendar.Joiner;

    /// <inheritdoc />
    public IHolidayCalendar Of(Name name)
    {
        if (HolidayCalendars.TryGetValue(name, out var calendar))
            return calendar;

        throw new NotImplementedException($"Unknown holiday calendar: {name}");
    }

    /// <inheritdoc />
    public IHolidayCalendar Combine(IHolidayCalendar calendar1, IHolidayCalendar calendar2)
    {
        if (calendar1.Equals(calendar2))
            return calendar1;

        if (calendar1 is NoHolidaysCalendar && calendar2 is NoHolidaysCalendar)
            return NoHolidays;

        if (calendar1 is NoHolidaysCalendar || calendar2 is NoHolidaysCalendar)
        {
            return calendar1 is NoHolidaysCalendar ? calendar2 : calendar1;
        }

        var combined = new CombinedHolidayCalendar(calendar1, calendar2);

        HolidayCalendars.TryAdd(combined.Name, combined);

        return combined;
    }

    /// <inheritdoc />
    public IHolidayCalendar Link(IHolidayCalendar calendar1, IHolidayCalendar calendar2)
    {
        if (calendar1.Equals(calendar2))
            return calendar1;

        if (calendar1 is NoHolidaysCalendar || calendar2 is NoHolidaysCalendar)
            return NoHolidays;

        var linked = new CombinedHolidayCalendar(calendar1, calendar2);

        HolidayCalendars.TryAdd(linked.Name, linked);

        return linked;
    }

    /// <inheritdoc />
    public IEnumerable<Name> HolidayCalendarNames() => HolidayCalendars.Keys;
}