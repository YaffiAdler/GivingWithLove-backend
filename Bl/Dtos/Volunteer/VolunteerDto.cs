namespace Bl.Dtos.Volunteer
{
    // List item. Names come from Eichud and are trimmed; FirstName and FamilyName stay separate on purpose.
    // The contact fields are the ones the all-volunteers table shows (owner's decision); text fields are trimmed.
    public sealed record VolunteerDto(
        int VolunteerCode,
        string? FirstName,
        string? FamilyName,
        int? PositionCode,
        string? PositionName,
        IReadOnlyList<int> ProjectCodes,
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
