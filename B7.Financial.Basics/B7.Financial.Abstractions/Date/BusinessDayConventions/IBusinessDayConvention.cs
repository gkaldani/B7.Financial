using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Abstractions.Date.BusinessDayConventions;

/// <summary>
/// A convention defining how to adjust a date if it falls on a day other than a business day.
/// <para>
/// The purpose of this convention is to define how to handle non-business days. <br/>
/// When processing dates in finance, it is typically intended that non-business days, <br/>
/// such as weekends and holidays, are converted to a nearby valid business day. <br/>
/// The convention, in conjunction with a <see cref="IHolidayCalendar"/>, <br/>
/// defines exactly how the adjustment should be made.
/// </para>
/// <para>
/// The most common implementations are provided in <see cref="IBusinessDayConventionFactory"/>. <br/>
/// Additional implementations may be added by implementing this interface.
/// </para>
/// </summary>
public interface IBusinessDayConvention : INamed
{
    /// <summary>
    /// Adjusts the date as necessary if it is not a business day.
    /// <para>
    /// If the date is a business day it will be returned unaltered. <br/>
    /// If the date is not a business day, the convention will be applied.
    /// </para>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <param name="calendar">The calendar that defines holidays and business days</param>
    /// <returns>The adjusted date</returns>
    DateOnly Adjust(DateOnly date, IHolidayCalendar calendar);
    
}