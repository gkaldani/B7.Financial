namespace B7.Financial.Abstractions.Date;

/// <inheritdoc />
public abstract class HolidayCalendar : IHolidayCalendar
{
    /// <inheritdoc/>
    public abstract Name Name { get; }

    /// <inheritdoc />
    public abstract bool IsHoliday(DateOnly date);

    /// <inheritdoc />
    public virtual bool IsBusinessDay(DateOnly date) => !IsHoliday(date);

    /// <inheritdoc />
    public virtual DateAdjuster AdjustBy(int days) => date => Shift(date, days);

    /// <inheritdoc />
    public virtual DateOnly Shift(DateOnly date, int days)
    {
        if (days == 0)
            return date; // No adjustment needed

        DateAdjuster shift = days > 0 ? Next : Previous;

        var adjusted = date;

        for (var i = 0; i < Math.Abs(days); i++)
            adjusted = shift(date);

        return adjusted;
    }

    /// <inheritdoc />
    public virtual DateOnly Next(DateOnly date) => NextOrSame(date.AddDays(1));

    /// <inheritdoc />
    public virtual DateOnly NextOrSame(DateOnly date)
    {
        var next = date;
        while (IsHoliday(next))
        {
            next = date.AddDays(1);
        }

        return next;    
    }

    /// <inheritdoc />
    public virtual DateOnly Previous(DateOnly date) => PreviousOrSame(date.AddDays(-1));

    /// <inheritdoc />
    public virtual DateOnly PreviousOrSame(DateOnly date)
    {
        var previous = date;
        while (IsHoliday(previous))
        {
            previous = date.AddDays(-1);
        }

        return previous;
    }

    /// <inheritdoc />
    public virtual DateOnly NextSameOrLastInMonth(DateOnly date)
    {
        var nextOrSame = NextOrSame(date);
        
        return (nextOrSame.Month != date.Month)
            ? Previous(date)
            : nextOrSame;
    }

    /// <inheritdoc />
    public virtual bool IsLastBusinessDayOfMonth(DateOnly date) =>
        IsBusinessDay(date) && Next(date).Month != date.Month;

    /// <inheritdoc />
    public virtual DateOnly LastBusinessDayOfMonth(DateOnly date) =>
        PreviousOrSame(new DateOnly(date.Year, date.Month, day: DateTime.DaysInMonth(date.Year, date.Month)));

    /// <inheritdoc />
    public virtual int DaysBetween(DateOnly startInclusive, DateOnly endExclusive) =>
        BusinessDays(startInclusive, endExclusive).Count();
    
    /// <inheritdoc />
    public virtual IEnumerable<DateOnly> BusinessDays(DateOnly startInclusive, DateOnly endExclusive)
    {
        if (startInclusive >= endExclusive)
            throw new ArgumentException("The start date must be earlier than the end date.");

        for (var date = startInclusive; date < endExclusive; date = date.AddDays(1))
        {
            if (IsBusinessDay(date))
            {
                yield return date;
            }
        }
    }

    /// <inheritdoc />
    public virtual IEnumerable<DateOnly> Holidays(DateOnly startInclusive, DateOnly endExclusive)
    {
        if (startInclusive >= endExclusive)
            throw new ArgumentException("The start date must be earlier than the end date.");

        for (var date = startInclusive; date < endExclusive; date = date.AddDays(1))
        {
            if (IsHoliday(date))
            {
                yield return date;
            }
        }
    }

    /// <inheritdoc />
    public bool Equals(IHolidayCalendar? other) => other is not null && Name.Equals(other.Name);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((HolidayCalendar)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Name.GetHashCode();
}