using Bl.Dtos.Common;
using Bl.Exceptions;
using Dal.Queries;

namespace Bl.Mapping
{
    internal static class PagingMapper
    {
        public static PageRequest ToPageRequest(PagingQueryDto query)
        {
            return new PageRequest(query.Page, query.PageSize);
        }

        // Without a direction the list's default for the chosen sort applies, so existing calls keep their order.
        public static SortDirection ToDirection(string? direction, SortDirection defaultDirection)
        {
            return direction?.Trim().ToLowerInvariant() switch
            {
                null or "" => defaultDirection,
                PagingQueryDto.Ascending => SortDirection.Ascending,
                PagingQueryDto.Descending => SortDirection.Descending,
                _ => throw new InvalidQueryException(
                    $"Unknown direction '{direction}'. Allowed values: {string.Join(", ", PagingQueryDto.DirectionValues)}.")
            };
        }

        public static PagedResult<TDto> ToPagedResult<TRow, TDto>(PagedRows<TRow> rows, PagingQueryDto query, Func<TRow, TDto> toDto)
        {
            return new PagedResult<TDto>(rows.Items.Select(toDto).ToList(), rows.TotalCount, query.Page, query.PageSize);
        }
    }
}
