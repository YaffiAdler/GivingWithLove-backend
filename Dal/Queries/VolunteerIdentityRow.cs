namespace Dal.Queries
{
    // Names are returned as stored (fixed-length, untrimmed).
    public sealed record VolunteerIdentityRow(int VolunteerCode, string? FirstName, string? FamilyName);
}
