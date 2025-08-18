using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Date;

namespace B7.Financial.Basics.Date.BusinessDayConventions;

/// <summary>
/// The 'Nearest' convention which adjusts <see cref="DayOfWeek.Sunday"/> and <see cref="DayOfWeek.Monday"/> forward, and other days backward.
/// <para>
/// If the input date is not a business day then the date is adjusted. <br/>
/// If the input is Sunday or Monday then the next business day is returned. <br/>
/// Otherwise, the previous business day is returned.
/// </para>
/// <remarks>
/// Note that despite the name, the algorithm may not return the business day that is actually nearest.
/// </remarks>
/// </summary>
public sealed class BusinessDayNearest : BusinessDayConvention
{
    /// <summary>
    /// The name of the Business Day Convention.
    /// </summary>
    public static readonly Name BusinessDayConventionName = "Nearest";


    /// <inheritdoc />
    public override Name Name => BusinessDayConventionName;

    /// <inheritdoc />
    public override DateOnly Adjust(DateOnly date, IHolidayCalendar calendar)
    {
        // Check if the date is a business day
        if (calendar.IsBusinessDay(date))
            return date;

        // next business day if Sun/Mon, otherwise previous
        return date.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Monday 
            ? calendar.Next(date) 
            : calendar.Previous(date);
    }
}