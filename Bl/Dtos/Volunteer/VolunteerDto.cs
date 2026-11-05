namespace Bl.Dtos.Volunteer
{
    // List item. Names come from Eichud and are trimmed; FirstName and FamilyName stay separate on purpose.
    public sealed record VolunteerDto(
        int VolunteerCode,
        string? FirstName,
        string? FamilyName,
        int? PositionCode,
        string? PositionName,
        IReadOnlyList<int> ProjectCodes);
}
