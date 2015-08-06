namespace PureRide.Web.Configuration
{
    public interface IScheduleSettings
    {
        int MaxVisibleDays { get; }
        int MaxBookableDays { get; }
        int MinBookableMinutesBeforeStart { get; }
    }
}