namespace Dal.Queries
{
    // Sums over the matching volunteerings. Dates are null when nothing matches.
    // DistinctVolunteers ignores rows without a volunteer.
    public sealed record TotalsRow(
        int Count,
        int DistinctVolunteers,
        double EstimatedTimeSum,
        double EstimatedCostSum,
        DateOnly? FirstDate,
        DateOnly? LastDate);
}
