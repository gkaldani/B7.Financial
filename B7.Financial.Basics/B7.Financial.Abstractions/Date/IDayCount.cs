namespace B7.Financial.Abstractions.Date;

/// <summary>
/// Common interface for day count conventions.
/// </summary>
public interface IDayCount : INamed
{
    /// <summary>
    /// Gets the year fraction between the specified dates. <br/>
    /// Given two dates, this method returns the fraction of a year between these <br/>
    /// dates according to the convention. The dates must be in order.
    /// </summary>
    /// <param name="firstDate">The first date</param>
    /// <param name="secondDate">The second date, on or after the <see cref="firstDate"/></param>
    /// <param name="scheduleInfo">The schedule information</param>
    /// <returns>The year fraction, zero or greater</returns>
    public decimal YearFraction(DateOnly firstDate, DateOnly secondDate, IScheduleInfo? scheduleInfo);

    /// <summary>
    /// Calculates the number of days between the specified dates using the rules of this day count.
    /// <para>
    /// A day count is typically defines as a count of days divided by a year estimate. <br/>
    /// This method returns the count of days, which is the numerator of the division. <br/>
    /// For example, the 'Act/Act' day count will return the actual number of days between <br/>
    /// the two dates, but the '30/360 ISDA' will return a value based on 30 day months.
    /// </para>
    /// </summary>
    /// <param name="firstDate">The first date</param>
    /// <param name="secondDate">The second date</param>
    /// <returns>The number of days, as determined by the day count</returns>
    public int Days(DateOnly firstDate, DateOnly secondDate);

    /// <summary>
    /// Information about the schedule necessary to calculate the day count.
    /// <para>
    /// Some <see cref="DayCount"/> implementations require additional information about the schedule. <br/>
    /// Implementations of this interface provide that information.
    /// </para>
    /// </summary>
    public interface IScheduleInfo
    {
        /// <summary>
        /// The start date of the schedule.
        /// <para>
        /// The first date in the schedule. <br/>
        /// If the schedule adjusts for business days, then this is the adjusted date.
        /// </para>
        /// </summary>
        public DateOnly StartDate { get; }

        /// <summary>
        /// The end date of the schedule.
        /// <para>
        /// The last date in the schedule. <br/>
        /// If the schedule adjusts for business days, then this is the adjusted date.
        /// </para>
        /// </summary>
        public DateOnly EndDate { get; }

        /// <summary>
        /// Gets the end date of the schedule period. <br/>
        /// This is called when a day count requires the end date of the schedule period.
        /// </summary>
        /// <param name="date">The date to find the period end date for</param>
        /// <returns>The period end date</returns>
        public DateOnly PeriodEndDate(DateOnly date);
    }
}