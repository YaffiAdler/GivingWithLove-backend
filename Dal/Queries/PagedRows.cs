namespace Dal.Queries
{
    // One page of rows plus the number of rows that match the query on all pages.
    public sealed record PagedRows<T>(IReadOnlyList<T> Items, int TotalCount);
}
