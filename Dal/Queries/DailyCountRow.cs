namespace Dal.Queries
{
    // Volunteerings of one calendar day. Used for the weekday chart: the weekday is taken from the date by the caller,
    // because SQL Server's DATEPART(weekday) depends on the session's DATEFIRST setting. At most one row per day of the range.
    public sealed record DailyCountRow(DateOnly Date, int Count);
}
