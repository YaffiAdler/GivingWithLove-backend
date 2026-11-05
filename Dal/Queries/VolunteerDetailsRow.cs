namespace Dal.Queries
{
    // VolunteerRow plus the personal and contact fields from Eichud.
    public sealed record VolunteerDetailsRow(
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
