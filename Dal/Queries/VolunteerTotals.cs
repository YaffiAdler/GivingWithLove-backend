namespace Dal.Queries
{
    // The aggregates of one volunteer before paging; member-initialized so EF can order and page on its members.
    internal sealed class VolunteerTotals
    {
        public int VolunteerCode { get; init; }

        public int Count { get; init; }

        public double EstimatedTimeSum { get; init; }

        public double EstimatedCostSum { get; init; }

        public DateOnly LastDate { get; init; }
    }
}
