using Dal.Queries;
using Microsoft.EntityFrameworkCore;

namespace Dal.Paging
{
    public static class QueryablePagingExtensions
    {
        // Exactly two SQL statements: COUNT over the filtered rows, then the ordered page with OFFSET/FETCH.
        // The count runs before ordering and projection, so it stays a plain COUNT over the filter.
        // The projection runs after Skip/Take, so related data is joined only for the rows of the page.
        public static async Task<PagedRows<TRow>> ToPagedRowsAsync<TSource, TRow>(
            this IQueryable<TSource> filtered,
            Func<IQueryable<TSource>, IOrderedQueryable<TSource>> orderBy,
            Func<IQueryable<TSource>, IQueryable<TRow>> project,
            PageRequest page,
            CancellationToken ct)
        {
            int totalCount = await filtered.CountAsync(ct);

            IQueryable<TSource> pageQuery = orderBy(filtered)
                .Skip(page.Skip)
                .Take(page.PageSize);

            List<TRow> items = await project(pageQuery).ToListAsync(ct);
            return new PagedRows<TRow>(items, totalCount);
        }
    }
}
