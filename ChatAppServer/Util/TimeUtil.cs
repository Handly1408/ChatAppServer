namespace ChatAppServer.Util
{
    public class TimeUtil
    {
        public static long GetCurrentTimeMilliseconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
        public static long GetRegionTime() {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(GetCurrentTimeMilliseconds());
            Console.WriteLine($"UTC: {dateTimeOffset.UtcDateTime}");

            TimeZoneInfo kievTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Kiev");
            DateTimeOffset kievTime = TimeZoneInfo.ConvertTime(dateTimeOffset, kievTimeZone);

            Console.WriteLine($"Your time zone (Kiev): {kievTime.LocalDateTime}");
            return kievTime.ToUnixTimeMilliseconds();

        }
    }
}
