namespace Bl.Dtos.Stats
{
    // Counts, hours and value are of the volunteerings the person did as the volunteer;
    // TotalAsMatcher counts the volunteerings the person arranged as the matcher.
    // FirstDate and LastDate are null when there is no volunteering in the range. ByWeekday always has 7 rows.
    public sealed record VolunteerStatsDto(
        int VolunteerCode,
        string? FirstName,
        string? FamilyName,
        int TotalVolunteerings,
        int TotalAsMatcher,
        decimal TotalHours,
        decimal TotalValue,
        DateOnly? FirstDate,
        DateOnly? LastDate,
        IReadOnlyList<MonthlyCountDto> ByMonth,
        IReadOnlyList<WeekdayCountDto> ByWeekday,
        IReadOnlyList<NamedCountDto> ByProject,
        IReadOnlyList<NamedCountDto> BySubProject);
}
