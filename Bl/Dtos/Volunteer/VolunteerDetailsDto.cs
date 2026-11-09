namespace Bl.Dtos.Volunteer
{
    // A single volunteer with the Eichud fields that the volunteer screens show; text fields are trimmed.
    // Today it has the same fields as VolunteerDto.
    public sealed record VolunteerDetailsDto(
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
