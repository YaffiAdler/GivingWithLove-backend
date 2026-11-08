namespace Bl.Dtos.Stats
{
    // DayOfWeek: 0 = Sunday ... 6 = Saturday.
    public sealed record WeekdayCountDto(int DayOfWeek, int Count);
}
