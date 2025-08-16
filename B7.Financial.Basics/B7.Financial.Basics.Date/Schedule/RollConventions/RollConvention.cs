using B7.Financial.Abstractions;
using B7.Financial.Abstractions.Schedule;

namespace B7.Financial.Basics.Date.Schedule.RollConventions;

/// <inheritdoc />
public abstract class RollConvention : IRollConvention
{
    /// <inheritdoc />
    public abstract Name Name { get; }


    /// <inheritdoc />
    public abstract DateOnly Adjust(DateOnly date);

    /// <inheritdoc />
    public virtual bool Matches(DateOnly date) => date == Adjust(date);
    

    /// <inheritdoc />
    public virtual DateOnly Next(DateOnly date, Frequency periodicFrequency)
    {
        var next = Adjust(periodicFrequency.AddTo(date));
        return next.IsAfter(date) ? next : Adjust(next.AddMonths(1));
    }

    /// <inheritdoc />
    public virtual DateOnly Previous(DateOnly date, Frequency periodicFrequency)
    {
        var previous = Adjust(periodicFrequency.SubtractFrom(date));
        return previous.IsBefore(date) ? previous : Adjust(previous.AddMonths(-1));
    }

    /// <inheritdoc />
    public virtual byte DayOfMonth => 0;
}