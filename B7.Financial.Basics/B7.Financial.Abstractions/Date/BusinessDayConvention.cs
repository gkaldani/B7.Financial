namespace B7.Financial.Abstractions.Date;

/// <inheritdoc />
public abstract class BusinessDayConvention : IBusinessDayConvention
{
    /// <inheritdoc />
    public abstract Name Name { get; }

    /// <inheritdoc />
    public abstract DateOnly Adjust(DateOnly date, IHolidayCalendar calendar);
}