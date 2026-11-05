using Bl.Dtos.Common;
using Dal.Queries;

namespace Bl.Mapping
{
    internal static class PagingMapper
    {
        public static PageRequest ToPageRequest(PagingQueryDto query)
        {
            return new PageRequest(query.Page, query.PageSize);
        }

        public static PagedResult<TDto> ToPagedResult<TRow, TDto>(PagedRows<TRow> rows, PagingQueryDto query, Func<TRow, TDto> toDto)
        {
            return new PagedResult<TDto>(rows.Items.Select(toDto).ToList(), rows.TotalCount, query.Page, query.PageSize);
        }
    }
}
