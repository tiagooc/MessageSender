namespace MessageSender;

public static class DatetimeExtensions
{
    public static TimeSpan DelayUntil(this DateTime datetime)
    {
        return datetime - DateTime.UtcNow;
    }
}