namespace Bl.Dtos.Stats
{
    // For a department: everything under its child projects (and volunteerings pointing to the department itself),
    // and ByChild lists the projects. For a project: its own volunteerings, and ByChild lists its sub-projects.
    public sealed record ProjectStatsDto(
        int ProjectCode,
        string ProjectName,
        bool IsDepartment,
        int TotalVolunteerings,
        int DistinctVolunteers,
        decimal TotalHours,
        decimal TotalValue,
        IReadOnlyList<MonthlyCountDto> ByMonth,
        IReadOnlyList<NamedCountDto> ByChild,
        IReadOnlyList<VolunteerRankingRowDto> TopVolunteers);
}
