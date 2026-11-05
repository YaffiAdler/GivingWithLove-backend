namespace Dal.Queries
{
    // A volunteer with the name from Eichud, the position name and the codes of the projects in the volunteer's domains.
    public sealed record VolunteerRow(
        int VolunteerCode,
        string? FirstName,
        string? FamilyName,
        int? PositionCode,
        string? PositionName,
        List<int> ProjectCodes);
}
