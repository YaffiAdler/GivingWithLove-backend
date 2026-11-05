namespace Bl.Dtos.Common
{
    // One page of a list plus TotalCount, the number of matching items on all pages.
    public sealed record PagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalCount,
        int Page,
        int PageSize);
}
