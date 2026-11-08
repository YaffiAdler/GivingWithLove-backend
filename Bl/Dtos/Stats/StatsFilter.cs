namespace Bl.Dtos.Stats
{
    // Date range of every statistics endpoint; both ends are optional and inclusive.
    // From later than To, or a range longer than 10 years (StatsMapper.MaxRangeYears), is rejected with 400.
    public sealed class StatsFilter
    {
        public DateOnly? From { get; init; }

        public DateOnly? To { get; init; }
    }
}
