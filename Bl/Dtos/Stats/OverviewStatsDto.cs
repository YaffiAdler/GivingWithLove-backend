namespace Bl.Dtos.Stats
{
    // TotalValue is in NIS. ByMonth covers every month of the range, with zero rows for months without activity.
    public sealed record OverviewStatsDto(
        int TotalVolunteerings,
        int ActiveVolunteers,
        decimal TotalHours,
        decimal TotalValue,
        IReadOnlyList<MonthlyCountDto> ByMonth,
        IReadOnlyList<NamedCountDto> ByDepartment,
        IReadOnlyList<NamedCountDto> ByProject,
        IReadOnlyList<NamedCountDto> BySubProject);
}
