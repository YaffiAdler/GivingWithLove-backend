using System.Linq.Expressions;
using Dal.Queries;

namespace Dal.Paging
{
    internal static class QueryableSortExtensions
    {
        public static IOrderedQueryable<T> OrderByDirection<T, TKey>(
            this IQueryable<T> source, Expression<Func<T, TKey>> keySelector, SortDirection direction)
        {
            return direction == SortDirection.Descending
                ? source.OrderByDescending(keySelector)
                : source.OrderBy(keySelector);
        }

        public static IOrderedQueryable<T> ThenByDirection<T, TKey>(
            this IOrderedQueryable<T> source, Expression<Func<T, TKey>> keySelector, SortDirection direction)
        {
            return direction == SortDirection.Descending
                ? source.ThenByDescending(keySelector)
                : source.ThenBy(keySelector);
        }
    }
}
