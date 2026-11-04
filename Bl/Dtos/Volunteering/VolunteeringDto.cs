namespace Bl.Dtos.Volunteering
{
    // Read model: the codes plus the display names; a name is null when its code points to no row.
    public sealed record VolunteeringDto(
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
