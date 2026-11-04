namespace Dal.Queries
{
    // A volunteering with the display names of the rows it points to; a name is null when its code has no matching row.
    public sealed record VolunteeringRow(
        int VolunteeringCode,
        DateOnly DateOfVolunteering,
        int? VolunteerCode,
        string? VolunteerName,
        int? MatcherCode,
        string? MatcherName,
        int? PoorManCode,
        string? PoorManName,
        int? ProjectCode,
        string? ProjectName,
        int? SubProjectCode,
        string? SubProjectName);
}
