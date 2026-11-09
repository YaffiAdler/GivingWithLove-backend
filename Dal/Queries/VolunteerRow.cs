namespace Dal.Queries
{
    // A volunteer with the personal and contact fields from Eichud, the position name and the codes of the projects
    // in the volunteer's domains. Used by both the list and the single-volunteer read, so the projection exists once.
    public sealed record VolunteerRow(
        int VolunteerCode,
        string? FirstName,
        string? FamilyName,
        int? PositionCode,
        string? PositionName,
        List<int> ProjectCodes,
        string? Tohar,
        string? Ending,
        string? FathersName,
        string? Shtibel,
        string? Street,
        string? House,
        string? City,
        int? ZipCode,
        string? HousePhone,
        string? CellPhone,
        string? Shver);
}
