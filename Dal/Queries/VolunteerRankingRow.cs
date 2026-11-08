namespace Dal.Queries
{
    // Names are null when the volunteer has no Eichud row; they are returned as stored (fixed-length, untrimmed).
    public sealed record VolunteerRankingRow(
        int VolunteerCode,
        string? FirstName,
        string? FamilyName,
        int Count,
        double EstimatedTimeSum,
        double EstimatedCostSum,
        DateOnly LastDate);
}
