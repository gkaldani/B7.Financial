namespace B7.Financial.Abstractions.Date;

/// <summary>
/// A holiday calendar implementation that combines two other calendars.
/// <para>
/// This immutable implementation of <see cref="IHolidayCalendar"/> stores two underlying calendars. <br/>
/// A date is a holiday if either calendar defines it as a holiday.
/// </para>
/// </summary>
public sealed class CombinedHolidayCalendar : HolidayCalendar
{
    /// <summary>
    /// Joiner character for combining two calendars.
    /// </summary>
    public const char Joiner  = '+';

    /// <summary>
    /// Calendar 1.
    /// </summary>
    public IHolidayCalendar Calendar1 { get; }

    /// <summary>
    /// Calendar 2.
    /// </summary>
    public IHolidayCalendar Calendar2 { get; }

    /// <summary>
    /// Combines two holiday calendars into one.
    /// </summary>
    /// <param name="calendar1"></param>
    /// <param name="calendar2"></param>
    public CombinedHolidayCalendar(IHolidayCalendar calendar1, IHolidayCalendar calendar2)
    {
        Calendar1 = calendar1;
        Calendar2 = calendar2;
    }

    /// <inheritdoc />
    public override Name Name => $"[{Calendar1.Name}+{Calendar2.Name}]";

    /// <inheritdoc />
    public override bool IsHoliday(DateOnly date) => Calendar1.IsHoliday(date) || Calendar2.IsHoliday(date);
}