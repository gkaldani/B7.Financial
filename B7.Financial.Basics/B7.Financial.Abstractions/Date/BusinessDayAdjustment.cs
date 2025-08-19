using B7.Financial.Abstractions.Date.BusinessDayConventions;
using B7.Financial.Abstractions.Date.HolidayCalendars;

namespace B7.Financial.Abstractions.Date;

/// <summary>
/// An adjustment that alters a date if it falls on a day other than a business day.
/// <para>
/// When processing dates in finance, it is typically intended that non-business days, <br/>
/// such as weekends and holidays, are converted to a nearby valid business day. <br/>
/// This class represents the necessary adjustment.
/// </para>
/// </summary>
public sealed class BusinessDayAdjustment
{
    /// <summary>
    /// An instance that performs no adjustment.
    /// </summary>
    public static readonly BusinessDayAdjustment NoAdjustment = new(BusinessDayNoAdjust.NoAdjust, NoHolidaysCalendar.NoHolidays);

    /// <summary>
    /// The convention used to the adjust the date if it does not fall on a business day.
    /// <para>
    /// The convention determines whether to move forwards or backwards when it is a holiday.
    /// </para>
    /// </summary>
    public IBusinessDayConvention BusinessDayConvention { get; }

    /// <summary>
    /// The calendar that defines holidays and business days.
    /// <para>
    /// When the adjustment is made, this calendar is used to skip holidays.
    /// </para>
    /// </summary>
    public IHolidayCalendar HolidayCalendar { get; }

    /// <summary>
    /// A DateAdjuster that adjusts a date according to the business day convention and holiday calendar.
    /// </summary>
    public DateAdjuster Adjuster { get; }

    /// <summary>
    /// Creates a new instance of <see cref="BusinessDayAdjustment"/> with the specified business day convention and holiday calendar.
    /// </summary>
    /// <param name="businessDayConvention"></param>
    /// <param name="holidayCalendar"></param>
    public BusinessDayAdjustment(IBusinessDayConvention businessDayConvention, IHolidayCalendar holidayCalendar)
    {
        BusinessDayConvention = businessDayConvention;
        HolidayCalendar = holidayCalendar;

        if (BusinessDayConvention == BusinessDayNoAdjust.NoAdjust && HolidayCalendar.Equals(NoHolidaysCalendar.NoHolidays))
        {
            Adjuster = date => date;
        }
        else
        {
            // Create the adjuster using the convention and calendar
            Adjuster = date => BusinessDayConvention.Adjust(date, HolidayCalendar);
        }
    }

    /// <summary>
    /// Adjusts the date as necessary if it is not a business day.
    /// <para>
    /// If the date is a business day it will be returned unaltered. <br/>
    /// If the date is not a business day, the convention will be applied.
    /// </para>
    /// </summary>
    /// <param name="date">The date to adjust</param>
    /// <returns>The adjusted date</returns>
    public DateOnly Adjust(DateOnly date) => Adjuster(date);

    /// <inheritdoc />
    public override string ToString()
    {
        if (this.Equals(NoAdjustment))
            return BusinessDayConvention.Name;

        return $"{BusinessDayConvention} using calendar {HolidayCalendar.Name}";
    }
}