namespace Dal.Queries
{
    // Page is 1-based; the caller has already validated both values.
    public sealed record PageRequest(int Page, int PageSize)
    {
        public int Skip => (Page - 1) * PageSize;
    }
}
