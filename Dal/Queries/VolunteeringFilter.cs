namespace Dal.Queries
{
    // Every criterion is optional; From and To are inclusive.
    public sealed record VolunteeringFilter(
        int? VolunteerCode,
        int? ProjectCode,
        DateOnly? From,
        DateOnly? To);
}
