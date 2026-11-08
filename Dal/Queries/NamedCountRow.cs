namespace Dal.Queries
{
    // Volunteerings of one department, project or sub-project.
    public sealed record NamedCountRow(int Code, string Name, int Count, double EstimatedTimeSum, double EstimatedCostSum);
}
