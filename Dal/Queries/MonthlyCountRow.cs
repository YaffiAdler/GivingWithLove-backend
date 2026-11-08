namespace Dal.Queries
{
    // Only months that have volunteerings; filling the gaps is the caller's job.
    public sealed record MonthlyCountRow(int Year, int Month, int Count, double EstimatedTimeSum);
}
