namespace Bl.Dtos.Volunteering
{
    // VolunteeringCode must equal the route id; the controller rejects a mismatch.
    public sealed record UpdateVolunteeringDto(
        int VolunteeringCode,
        DateOnly DateOfVolunteering,
        int? VolunteerCode,
        int? MatcherCode,
        int? PoorManCode,
        int? ProjectCode,
        int? SubProjectCode);
}
