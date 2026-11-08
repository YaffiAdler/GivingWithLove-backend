namespace Bl.Dtos.Stats
{
    // Names are trimmed and null when the volunteer has no Eichud row. Value is in NIS.
    public sealed record VolunteerRankingRowDto(
        int VolunteerCode,
        string? FirstName,
        string? FamilyName,
        int Count,
        decimal Hours,
        decimal Value,
        DateOnly LastDate);
}
